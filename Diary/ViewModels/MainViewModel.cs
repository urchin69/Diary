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


        private Student _selectedStudent;

    public Student SelectedStudent
    {
        get { return _selectedStudent; }
        set
        {
            _selectedStudent = value;
            OnPropertyChanged();
        }
    }




        private ObservableCollection<Student> _students;

    public ObservableCollection<Student> Students
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

    private ObservableCollection<Group> _group;

        public ObservableCollection<Group> Groups
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
        Groups = new ObservableCollection<Group>
            {
                new Group{Id=0, Name="Wszystkie"},
                new Group{Id=1, Name="1A"},
               new Group{Id=2,  Name="2A"},
            };
        _selectedGroupId = 0;
    }


    private void RefreshDiary()
        {

            Students = new ObservableCollection<Student>
            {
                new Student
                {
                    FirstName= "Kazimierz",
                    LastName="Szpin",
                    Group= new Group { Id = 1 }
                },
                new Student
                {
                    FirstName= "Aleks",
                    LastName="Falek",
                    Group= new Group { Id = 2 }
                },
                 new Student
                {
                    FirstName= "Borys",
                    LastName="Snik",
                    Group= new Group { Id = 1 }
                },

             };

        }

        private void RefreStudents(object obj)

        {
            RefreshDiary();
        }

        private bool CanRefreshStudents(object obj)
        {
            return true;
        }

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
            var addEditStudentWindow = new AddEditStudentView(obj as Student);
            addEditStudentWindow.Closed += AddEditStudentWindow_Closed;
            addEditStudentWindow.ShowDialog();
        }

        private void AddEditStudentWindow_Closed(object sender, EventArgs e)
        {
            RefreshDiary();
        }


    }
}
