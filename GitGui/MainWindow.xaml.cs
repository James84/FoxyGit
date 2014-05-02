using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using GitGui.Business.Queries;

namespace GitGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private string workingDirectory = string.Empty;
        private readonly string gitExe = ConfigurationManager.AppSettings["git-path"];
        private readonly QueryGit queryGit = new QueryGit();

        public MainWindow()
        {
            InitializeComponent();

#if DEBUG
            //workingDirectory = @"C:\git\public\jobseeker";
            //queryGit.SetRepo(workingDirectory, gitExe);
            //InfoBlock.Text = workingDirectory;
            //InfoBlock.Visibility = Visibility.Visible;
#endif
        }

        private void Repo_Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(workingDirectory)) return;

            var dialog = new FolderBrowserDialog();

            var dialogResult = dialog.ShowDialog();

            workingDirectory = InfoBlock.Text = dialog.SelectedPath;

            if (string.IsNullOrEmpty(workingDirectory)) return;

            queryGit.SetRepo(gitExe, workingDirectory);
            InfoBlock.Visibility = Visibility.Visible;
        }

        private void Branch_Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(workingDirectory)) return;

            var branch = queryGit.GetCurrentBranch();

            InfoBlock.Text = string.Format("Your current branch is: {0}", branch);
            InfoBlock.Visibility = Visibility.Visible;
        }

        private void Branches_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(workingDirectory)) return;

            var branches = queryGit.GetAllBranches().ToList();

            if (!branches.Any()) 
                return;

            InfoBlock.Text += "\n Available branches: ";

            foreach (var branch in branches)
                InfoBlock.Text += string.Format("\n {0}", branch);
        }

        private void Diff_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(workingDirectory)) return;

            var diff = queryGit.GetModifiedFiles().ToList();

            if (!diff.Any())
                return;

            foreach (var branch in diff)
                InfoBlock.Text += string.Format("\n {0}", branch);

        }
    }
}
