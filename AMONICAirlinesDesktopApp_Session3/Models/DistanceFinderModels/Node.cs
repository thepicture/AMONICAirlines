namespace AMONICAirlinesDesktopApp_Session3.Models.DistanceFinderModels
{
    /// <summary>
    /// Определяет узел для алгоритма Дейкстры.
    /// </summary>
    /// <typeparam name="T">Тип вершины.</typeparam>
    public abstract class Node<T>
    {
        public T Vertex { get; set; }
        public int TentativeDistance { get; set; }
        public Node<T> Parent { get; set; }
    }
}
