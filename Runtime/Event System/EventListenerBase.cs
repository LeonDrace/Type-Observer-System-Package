using System;

namespace LeonDrace.TypeObserverEventSystem
{
	/// <summary>
	/// Should be the base class of almost every <see cref="EventListener{T}"/>.
	/// It uses a delegate array rather than an event to reduce gc.
	/// In case of 10000 listeners added it only creates 1.5mb garbage compared to an event delegate which creates 630mb.
	/// The add and remove listener is faster too.
	/// </summary>
	/// <typeparam name="TDelegate"></typeparam>
	/// In the future it might make sense to add bindings which contain the index which would change the remove from O(n) to O(1).
	public abstract class EventListenerBase<TDelegate> where TDelegate : class
	{
		protected TDelegate[] m_Listeners = new TDelegate[1];
		protected uint m_Count = 0;
		protected uint m_Capacity = 1;

		protected static IndexOutOfRangeException s_IndexOutOfRangeException = new IndexOutOfRangeException("Fewer listeners than expected.");

		public uint Count => m_Count;

		#region Contains

		public bool Contains(TDelegate listener)
		{
			return this.Contains(m_Listeners, m_Count, listener);
		}

		private bool Contains(TDelegate[] array, uint count, TDelegate listener)
		{
			for (uint i = 0; i < count; ++i)
			{
				if (array[i].Equals(listener))
				{
					return true;
				}
			}
			return false;
		}

		#endregion

		#region Add Listener

		public bool AddListener(TDelegate listener, bool allowDuplicates = true)
		{
			if (!allowDuplicates && Contains(listener)) return false;

			if (m_Count == m_Capacity)
			{
				m_Capacity *= 2;
				m_Listeners = Expand(m_Listeners, m_Capacity, m_Count);
			}

			m_Listeners[m_Count] = listener;
			m_Count++;

			return true;
		}

		#endregion

		#region Remove Listener

		public bool RemoveListener(TDelegate listener)
		{
			bool result = false;
			for (uint i = 0; i < m_Count; ++i)
			{
				if (m_Listeners[i].Equals(listener))
				{
					RemoveAt(i);
					result = true;
					break;
				}
			}

			return result;
		}

		public void RemoveAll()
		{
			Array.Clear(m_Listeners, 0, (int)m_Capacity);
			m_Count = 0;
		}

		#endregion

		#region Internal

		private TDelegate[] Expand(TDelegate[] array, uint capacity, uint count)
		{
			TDelegate[] newArr = new TDelegate[capacity];
			for (int i = 0; i < count; ++i)
			{
				newArr[i] = array[i];
			}
			return newArr;
		}

		protected void RemoveAt(uint i)
		{
			m_Count = RemoveAt(m_Listeners, m_Count, i);
		}

		protected uint RemoveAt(TDelegate[] array, uint count, uint i)
		{
			--count;
			for (uint j = i; j < count; ++j)
			{
				array[j] = array[j + 1];
			}
			array[count] = null;
			return count;
		}

		#endregion
	}
}


