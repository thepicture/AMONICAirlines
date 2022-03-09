using AMONICAirlinesDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMONICAirlinesDesktopApp.ViewModels
{
    public class AddUserViewModel : BaseViewModel
    {
        public AddUserViewModel()
        {
            Title = "Add user";
            Offices = Task.Run(() =>
            {
                using (BaseEntities context = new BaseEntities())
                {
                    return context.Office.ToList();
                }
            }).Result;
            CurrentOffice = Offices.FirstOrDefault();
        }

        private string email;

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string firstName;

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        private string lastName;

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        private object currentOffice;

        public object CurrentOffice
        {
            get => currentOffice;
            set => SetProperty(ref currentOffice, value);
        }

        private System.DateTime? birthDate;

        public System.DateTime? BirthDate
        {
            get => birthDate;
            set => SetProperty(ref birthDate, value);
        }

        private IEnumerable<Office> offices;

        public IEnumerable<Office> Offices
        {
            get => offices;
            set => SetProperty(ref offices, value);
        }
    }
}
