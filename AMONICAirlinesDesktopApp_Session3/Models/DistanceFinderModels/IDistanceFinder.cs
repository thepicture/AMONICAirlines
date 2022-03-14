namespace AMONICAirlinesDesktopApp_Session3.Models.DistanceFinderModels
{
    /// <summary>
    /// Определяет метод для подсчёта числа 
    /// количества остановок.
    /// </summary>
    /// <typeparam name="TTarget">Тип начальной и 
    /// конечной точки.</typeparam>
    public interface IDistanceFinder<TTarget>
    {
        /// <summary>
        /// Получает число остановок между целями.
        /// </summary>
        /// <param name="target1">Первая цель.</param>
        /// <param name="target2">Вторая цель.</param>
        /// <returns>Число остановок.</returns>
        int GetNumberOfStops(TTarget target1, TTarget target2);
    }
}
