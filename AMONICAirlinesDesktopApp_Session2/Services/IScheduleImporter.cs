namespace AMONICAirlinesDesktopApp_Session2.Services
{
    /// <summary>
    /// Определяет метод для импорта файла рейсов.
    /// </summary>
    public interface IScheduleImporter
    {
        int SuccessfulChangesCount { get; set; }
        int DuplicateRecordsCount { get; set; }
        int RecordsWithMissingFieldsCount { get; set; }
        /// <summary>
        /// Импортирует файл с рейсами из указанного пути.
        /// </summary>
        /// <param name="filePath">Путь импортируемого файла с рейсами.</param>
        /// <returns><see langword="true"/>, если файл существует, 
        /// иначе <see langword="false"/>.</returns>
        bool Import(string filePath);
    }
}
