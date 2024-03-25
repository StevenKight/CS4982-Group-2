using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CapstoneGroup2.Desktop.Library.Model;
using CapstoneGroup2.Desktop.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CapstoneGroup2.Desktop
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MySourcesPage : Page
    {
        #region Data members

        private List<Source> _sources;

        private readonly SourceViewModel _sourceViewModel;

        private List<Tag> _tags;

        #endregion

        #region Constructors

        public MySourcesPage()
        {
            this.InitializeComponent();

            this._sourceViewModel = new SourceViewModel();
            this._tags = new List<Tag>();
        }

        #endregion

        #region Methods

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.SearchButton.Visibility = Visibility.Collapsed;
            this.ClearButton.Visibility = Visibility.Collapsed;
            await this.loadSources();
            await this.loadTags();
        }

        private async Task loadSources()
        {
            var sourcesEnumerable = await this._sourceViewModel.GetSources(); // TODO: Add the shared sources
            this._sources = sourcesEnumerable.ToList();

            this.sourcesListBox.ItemsSource = this._sources; // TODO: Order by date
        }

        private async Task loadTags()
        {
            var tagsEnumerable = await this._sourceViewModel.GetTags();
            this._tags = tagsEnumerable.ToList();
            this.TagsComboBox.ItemsSource = this._tags;
        }

        private void sharedSourcesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(NotesPage), this._sources[this.sourcesListBox.SelectedIndex]);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddSourceDialog();
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var source = dialog.NewSource;
                await this._sourceViewModel.addNewSource(source);

                await this.loadSources();
            }
        }

        #endregion

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.TagsComboBox.SelectedValue != null)
            {
                var tags = new List<Tag>();
                tags.Add(TagsComboBox.SelectedValue as Tag);
                var sourcesEnumerable =  await this._sourceViewModel.GetSourcesByTags(tags);
                this.sourcesListBox.ItemsSource = sourcesEnumerable.ToList();
                this.ClearButton.Visibility = Visibility.Visible;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.TagsComboBox.SelectedValue = null;
            this.TagsComboBox.SelectedIndex = -1;
            this.sourcesListBox.ItemsSource = this._sources;
            this.ClearButton.Visibility = Visibility.Collapsed;
            this.SearchButton.Visibility = Visibility.Collapsed;
        }
        private void TagsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.TagsComboBox.SelectedValue != null)
            {
                this.SearchButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.SearchButton.Visibility = Visibility.Collapsed;
            }
        }

        private void TagsCombo(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}