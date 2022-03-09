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

        /// <summary>
        /// Представляет модальное окно для модели представления с параметром.
        /// </summary>
        /// <typeparam name="TViewModel">Модель представления.</typeparam>
        /// <typeparam name="TParam">Тип параметра.</typeparam>
        /// <param name="param">Параметр для модели представления.</param>
        void ShowModalWindowWithParameter<TViewModel, TParam>(TParam param)
            where TViewModel : BaseViewModel;

        /// <summary>
        /// Представляет модальное окно для модели представления с параметром.
        /// </summary>
        /// <typeparam name="TViewModel">Модель представления.</typeparam>
        /// <typeparam name="TParam">Тип параметра.</typeparam>
        /// <param name="param">Параметр для модели представления.</param>
        void ShowWindowWithParameter<TViewModel, TParam>(TParam param)
              where TViewModel : BaseViewModel;
    }
}
