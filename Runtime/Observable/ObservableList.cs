using System;
using System.Collections;
using System.Collections.Generic;

namespace LeonDrace.ObserverEventSystem.Observables
{
	/// <summary>
	/// Initializes a new instance of the ObservableList class that is empty
	/// or contains elements copied from the specified list.
	/// </summary>
	/// <param name="initialList"> The list to copy elements from. </param>    
	public interface IObservableList<T>
	{
		/// <summary>
		/// Adds an item to the ObservableList.
		/// </summary>
		/// <param name="item">
		/// The item to add.
		/// </param>        
		void Add(T item);

		/// <summary>
		/// Inserts an item into the ObservableList at the specified index.
		/// </summary>
		/// <param name="index"> The zero-based index at which the item should be inserted. </param>
		/// <param name="item"> The item to insert into the ObservableList. </param>        
		void Insert(int index, T item);

		/// <summary>
		/// Removes all items from the ObservableList.
		/// </summary>        
		void Clear();

		/// <summary>
		/// Determines whether the ObservableList contains a specific item.
		/// </summary>
		/// <param name="item"> The item to locate. </param>        
		bool Contains(T item);

		/// <summary>
		/// Determines the index of a specific item in the IonObservableList.
		/// </summary>
		/// <param name="item"> The item to locate in the IonObservableList. </param>
		int IndexOf(T item);

		/// <summary>
		/// Copies the elements of the ObservableList to an array, starting at a particular index.
		/// </summary>
		/// <param name="array"> The one dimensional array that is the destination of the elements
		/// copied from the ObservableList. </param>
		/// <param name="arrayIndex"> The zero-based index in the array at which copying begins. </param>        
		void CopyTo(T[] array, int arrayIndex);

		/// <summary>
		/// Removes the first occurrence of a specific item from the ObservableList.
		/// </summary>
		/// <param name="item"> The item to remove from the ObservableList. </param>        
		bool Remove(T item);

		/// <summary>
		/// Returns a generic enumerator that iterates through the ObservableList.
		/// </summary>
		/// <returns>A generic enumerator that can be used to iterate through the collection.</returns>        
		IEnumerator<T> GetEnumerator();

		/// <summary>
		/// Removes the item at the specified index of the ObservableList.
		/// </summary>
		/// <param name="index"> The zero-based index of the item to remove. </param>        
		void RemoveAt(int index);
	}

	[Serializable]
	public class ObservableList<T> : IList<T>, IObservableList<T>
	{
		private readonly IList<T> m_List;
		public event Action<IList<T>> AnyValueChanged;

		public ObservableList(IList<T> initialList = null)
		{
			m_List = initialList ?? new List<T>();
		}

		public T this[int index]
		{
			get => m_List[index];
			set
			{
				m_List[index] = value;
				Invoke();
			}
		}

		public void Invoke() => AnyValueChanged?.Invoke(m_List);

		public int Count => m_List.Count;

		public bool IsReadOnly => m_List.IsReadOnly;

		public void Add(T item)
		{
			m_List.Add(item);
			Invoke();
		}

		public void Clear()
		{
			m_List.Clear();
			Invoke();
		}

		public bool Contains(T item) => m_List.Contains(item);

		public void CopyTo(T[] array, int arrayIndex) => m_List.CopyTo(array, arrayIndex);

		public bool Remove(T item)
		{
			var result = m_List.Remove(item);
			if (result)
			{
				Invoke();
			}

			return result;
		}

		public IEnumerator<T> GetEnumerator() => m_List.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => m_List.GetEnumerator();

		public int IndexOf(T item) => m_List.IndexOf(item);

		public void Insert(int index, T item)
		{
			m_List.Insert(index, item);
			Invoke();
		}

		public void RemoveAt(int index)
		{
			T item = m_List[index];
			m_List.RemoveAt(index);
			Invoke();
		}
	}
}