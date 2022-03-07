using AMONICAirlinesDesktopApp.ViewModels;

namespace AMONICAirlinesDesktopApp.Services
{
    /// <summary>
    /// Определяет метод для представления окна.
    /// </summary>
    public interface IWindowService
    {
        /// <summary>
        /// Представляет окно для модели представления.
        /// </summary>
        /// <typeparam name="TViewModel">Модель представления.</typeparam>
        void ShowWindow<TViewModel>() where TViewModel : BaseViewModel, new();
    }
}
