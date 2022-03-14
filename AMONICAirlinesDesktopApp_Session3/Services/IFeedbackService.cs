namespace AMONICAirlinesDesktopApp_Session3.Services
{
    /// <summary>
    /// Определяет методы обратной связи.
    /// </summary>
    public interface IFeedbackService
    {
        /// <summary>
        /// Информирует.
        /// </summary>
        /// <param name="message">Информация.</param>
        void Inform(string message);
        /// <summary>
        /// Спрашивает.
        /// </summary>
        /// <param name="question">Вопрос.</param>
        /// <returns></returns>
        bool Ask(string question);
        /// <summary>
        /// Предупреждает.
        /// </summary>
        /// <param name="message">Предупреждение.</param>
        void Warn(string message);
        /// <summary>
        /// Оповещает об ошибке.
        /// </summary>
        /// <param name="message">Информация об ошибке.</param>
        void InformError(string message);
    }
}
