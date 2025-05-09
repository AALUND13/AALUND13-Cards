
namespace AALUND13Card.MonoBehaviours.PathFinding.Heap {
    public class Heap<T> where T : IHeapItem<T> {
        T[] items;
        int currentItemCount;

        public Heap(int maxHeapSize) {
            items = new T[maxHeapSize];
        }

        public void Add(T item) {
            item.HeapIndex = currentItemCount;
            items[currentItemCount] = item;
            SortUp(item);
            currentItemCount++;
        }

        public T RemoveFirst() {
            T firstItem = items[0];
            currentItemCount--;
            items[0] = items[currentItemCount];
            items[0].HeapIndex = 0;
            SortDown(items[0]);
            return firstItem;
        }

        public void UpdateItem(T item) {
            SortUp(item);
        }

        public int Count => currentItemCount;

        public bool Contains(T item) {
            return Equals(items[item.HeapIndex], item);
        }

        void SortDown(T item) {
            while(true) {
                int leftChild = item.HeapIndex * 2 + 1;
                int rightChild = item.HeapIndex * 2 + 2;
                int swapIndex = -1;

                if(leftChild < currentItemCount) {
                    swapIndex = leftChild;
                    if(rightChild < currentItemCount &&
                        items[rightChild].CompareTo(items[leftChild]) < 0) {
                        swapIndex = rightChild;
                    }

                    if(items[swapIndex].CompareTo(item) < 0) {
                        Swap(item, items[swapIndex]);
                    } else return;
                } else return;
            }
        }

        void SortUp(T item) {
            int parentIndex = (item.HeapIndex - 1) / 2;
            while(true) {
                T parent = items[parentIndex];
                if(item.CompareTo(parent) < 0) {
                    Swap(item, parent);
                } else break;

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        void Swap(T a, T b) {
            items[a.HeapIndex] = b;
            items[b.HeapIndex] = a;
            int aIndex = a.HeapIndex;
            a.HeapIndex = b.HeapIndex;
            b.HeapIndex = aIndex;
        }
    }
}