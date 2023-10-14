using Diary.Commands;
using Diary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Diary.Views;
using Diary.Models.Wrappers;

namespace Diary.ViewModels 

{
   public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {

            AddStudentCommand = new RelayCommand(AddEditStudent);
            EditStudentCommand = new RelayCommand(AddEditStudent, CanEditDeleteStudent);
            DeleteStudentCommand = new AsyncRelayCommand(DeleteStudent, CanEditDeleteStudent);
            RefreshStudentsCommand = new RelayCommand(RefreStudents);
            
            RefreshDiary();
            InitGroups();
          

        }

       

        public ICommand RefreshStudentsCommand { get; set; }

        public ICommand AddStudentCommand { get; set; }
        public ICommand EditStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }


        private StudentWrapper _selectedStudent;

    public StudentWrapper SelectedStudent
    {
        get { return _selectedStudent; }
        set
        {
            _selectedStudent = value;
            OnPropertyChanged();
        }
    }




        private ObservableCollection<StudentWrapper> _students;

    public ObservableCollection<StudentWrapper> Students
    {
        get { return _students; }
        set
        {
            _students = value;
            OnPropertyChanged();
        }
    }

    private int _selectedGroupId;

    public int SelectedGroupId
    {
        get { return _selectedGroupId; }
        set
        {
            _selectedGroupId = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<GroupWrapper> _group;

        public ObservableCollection<GroupWrapper> Groups
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged();
            }
        }



    private void InitGroups()
    {
            using (var context = new ApplicationDbContext())

            {
                var groups = context.Groups.ToList();
            }


            Groups = new ObservableCollection<GroupWrapper>
            {
                new GroupWrapper { Id = 0, Name = "Wszystkie" },
                new GroupWrapper{Id=1, Name="1A"},
               new GroupWrapper{Id=2,  Name="2A"},
            };
        _selectedGroupId = 0;
    }


    private void RefreshDiary()
        {

            Students = new ObservableCollection<StudentWrapper>
            {
                new StudentWrapper
                {
                    Id=1,
                    FirstName= "Kazimierz",
                    LastName="Szpin",
                    Group= new GroupWrapper { Id = 1 }
                },
                new StudentWrapper
                {
                    Id=2,
                    FirstName= "Aleks",
                    LastName="Falek",
                    Group= new GroupWrapper { Id = 2 }
                },
                 new StudentWrapper
                {
                     Id=3,
                    FirstName= "Borys",
                    LastName="Snik",
                    Group= new GroupWrapper { Id = 1 }
                },

             };

        }

        private void RefreStudents(object obj)

        {
            RefreshDiary();
        }

        //private bool CanRefreshStudents(object obj)
        //{
        //    return true;
        //}

        private bool CanEditDeleteStudent(object obj)
        {
            return SelectedStudent != null;
        }

        private async Task DeleteStudent(object obj)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            var dialog = await metroWindow.ShowMessageAsync("Usuwanie ucznia",
                $"Czy na pewno chcesz usunąć ucznia {SelectedStudent.FirstName}   " +
                $"{SelectedStudent.LastName}?", 
                MessageDialogStyle.AffirmativeAndNegative);

            if (dialog != MessageDialogResult.Affirmative)
                return;
            //usuwanie ucznia z bazy
            RefreshDiary();
        }

        private void AddEditStudent(object obj)
        {
            var addEditStudentWindow = new AddEditStudentView(obj as StudentWrapper);
            addEditStudentWindow.Closed += AddEditStudentWindow_Closed;
            addEditStudentWindow.ShowDialog();
        }

        private void AddEditStudentWindow_Closed(object sender, EventArgs e)
        {
            RefreshDiary();
        }


    }
}
