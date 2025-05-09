using System;

namespace AALUND13Card.MonoBehaviours.PathFinding.Heap {
    public interface IHeapItem<T> : IComparable<T> {
        int HeapIndex { get; set; }
    }
}
