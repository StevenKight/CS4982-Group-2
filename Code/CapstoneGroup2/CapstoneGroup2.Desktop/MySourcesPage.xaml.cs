using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            this.SearchButton.IsEnabled = false;
            this.ClearButton.IsEnabled = false;
            await this.loadSources();
            await this.loadTags();
        }

        private async Task loadSources()
        {
            var sourcesEnumerable = await this._sourceViewModel.GetSources();
            this._sources = sourcesEnumerable.ToList();

            this.sourcesListBox.ItemsSource = this._sources;
        }

        private async Task loadTags()
        {
            var tagsEnumerable = await this._sourceViewModel.GetTags();
            this._tags = tagsEnumerable.ToList();
            this.TagsListView.ItemsSource = this._tags;
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
            if (this.TagsListView.SelectedItems.Count > 0)
            {
                var tags = new List<Tag>();
                foreach (var tag in this.TagsListView.SelectedItems)
                {
                    tags.Add(tag as Tag);
                }
                tags.Add(TagsListView.SelectedValue as Tag);
                var sourcesEnumerable =  await this._sourceViewModel.GetSourcesByTags(tags);
                this.sourcesListBox.ItemsSource = sourcesEnumerable.ToList();
                this.ClearButton.IsEnabled = true;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.TagsListView.SelectedItems.Clear();
            this.TagsListView.SelectedIndex = -1;
            this.sourcesListBox.ItemsSource = this._sources;
            this.SearchButton.IsEnabled = false;
            this.ClearButton.IsEnabled = false;
        }
        private void TagsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.TagsListView.SelectedItems.Count > 0)
            {
                this.SearchButton.IsEnabled = true;
            }
            else
            {
                this.SearchButton.IsEnabled = false;
            }
        }

        private void TagsCombo(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}