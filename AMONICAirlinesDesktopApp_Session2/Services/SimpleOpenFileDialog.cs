using Microsoft.Win32;

namespace AMONICAirlinesDesktopApp_Session2.Services
{
    public class SimpleOpenFileDialog : IOpenFileDialog
    {
        public string Path { get; private set; }

        public bool ShowDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? result = dialog.ShowDialog();
            if (result != null
                && (bool)result)
            {
                Path = dialog.FileName;
            }
            return !string.IsNullOrWhiteSpace(Path);
        }
    }
}
