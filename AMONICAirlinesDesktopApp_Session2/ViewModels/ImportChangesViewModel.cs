using AMONICAirlinesDesktopApp.Commands;
using AMONICAirlinesDesktopApp_Session2.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AMONICAirlinesDesktopApp_Session2.ViewModels
{
    public class ImportChangesViewModel : BaseViewModel
    {
        public IScheduleImporter ScheduleImporter => DependencyService
                .Get<IScheduleImporter>();
        public ImportChangesViewModel()
        {
            Title = "Apply Schedule Changes";
        }

        private Command importFileCommand;

        public ICommand ImportFileCommand
        {
            get
            {
                if (importFileCommand == null)
                {
                    importFileCommand = new Command(
                        ImportFileAsync,
                        CanImportFileAsyncExecute);
                }

                return importFileCommand;
            }
        }

        /// <summary>
        /// Определяет, можно ли импортировать файл с рейсами.
        /// </summary>
        /// <returns><see langword="true"/>, 
        /// если можно импортировать 
        /// файл с рейсами, иначе <see langword="false"/>.</returns>
        private bool CanImportFileAsyncExecute(object arg)
        {
            return !IsBusy;
        }

        /// <summary>
        /// Импортирует файл с рейсами.
        /// </summary>
        private async void ImportFileAsync(object commandParameter)
        {
            IOpenFileDialog dialog = DependencyService
                .Get<IOpenFileDialog>();
            try
            {
                if (dialog.ShowDialog())
                {
                    ImportPath = dialog.Path;
                    IsBusy = true;
                    bool isImported = await Task.Run(() =>
                    {
                        return ScheduleImporter.Import(dialog.Path);
                    });
                    IsBusy = false;
                    if (isImported)
                    {
                        FeedbackService.Inform("Данные импортированы");
                        (App
                            .Current
                            .MainWindow
                            .DataContext as FlightScheduleViewModel)
                            .FilterSchedulesCommand
                            .Execute(null);
                    }
                    else
                    {
                        FeedbackService.Inform("Файл не существует");
                    }
                }
                else
                {
                    ImportPath = null;
                    FeedbackService.Inform("Операция была отменена");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                FeedbackService.InformError("Не удалось " +
                    "импортировать данные. " +
                    "Вероятно, файл поверждён");
            }
        }

        private string importPath;

        public string ImportPath
        {
            get => importPath;
            set => SetProperty(ref importPath, value);
        }
    }
}