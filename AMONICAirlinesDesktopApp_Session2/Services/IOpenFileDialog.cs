namespace AMONICAirlinesDesktopApp_Session2.Services
{
    /// <summary>
    /// Определяет метод для открытия файла.
    /// </summary>
    public interface IOpenFileDialog
    {
        /// <summary>
        /// Показывает диалог выбора файла.
        /// </summary>
        /// <returns><see langword="true"/>, 
        /// если файл выбран, иначе <see langword="false"/>.</returns>
        bool ShowDialog();
        /// <summary>
        /// Выбранный путь.
        /// </summary>
        string Path { get; }
    }
}
