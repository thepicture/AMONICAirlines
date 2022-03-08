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

        /// <summary>
        /// Представляет модальное окно для модели представления.
        /// </summary>
        /// <typeparam name="TViewModel">Модель представления.</typeparam>
        void ShowModalWindow<TViewModel>()
            where TViewModel : BaseViewModel, new();
    }
}
