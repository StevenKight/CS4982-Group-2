using System;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using CapstoneGroup2.Desktop.Data;
using CapstoneGroup2.Desktop.Library.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CapstoneGroup2.Desktop
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddSourceDialog : ContentDialog
    {
        #region Data members

        private StorageFile storageFile;

        #endregion

        #region Properties

        public Source NewSource { get; set; }

        #endregion

        #region Constructors

        public AddSourceDialog()
        {
            this.InitializeComponent();

            this.sourceIsLinkCheckBox.IsChecked = true;
        }

        #endregion

        #region Methods

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender,
            ContentDialogButtonClickEventArgs args)
        {
            var authorsCollection = this.sourceAuthorsListView.Items.ToList();
            var authorsObjects = authorsCollection?.Where(x =>
                x is User user && !user.Username.Equals("Click to edit") &&
                !string.IsNullOrWhiteSpace(user.Username));
            var authorsNames = authorsObjects?.Select(x => ((User)x).Username).ToList();

            Source source;

            switch (this.sourceIsVideoCheckBox.IsChecked)
            {
                case null:
                {
                    return;
                }
                case false:
                {
                    source = new Source
                    {
                        Type = SourceType.Pdf.ToString(), // TODO: Add type selection to UI for other types
                        Name = this.sourceNameTextBox.Text,
                        Description = this.sourceDescriptionTextBox.Text,
                        IsLink = this.sourceIsLinkCheckBox.IsChecked ?? true,
                        AuthorsString = string.Join("|", authorsNames),
                        Publisher = this.sourcePublisherTextBox.Text,
                        AccessedAt = this.sourceAccessedDatePicker.Date.DateTime
                    };
                    break;
                }
                default:
                {
                    source = new Source
                    {
                        Type = SourceType.Vid.ToString(), // TODO: Add type selection to UI for other types
                        Name = this.sourceNameTextBox.Text,
                        Description = this.sourceDescriptionTextBox.Text,
                        IsLink = this.sourceIsLinkCheckBox.IsChecked ?? true,
                        AuthorsString = string.Join("|", authorsNames),
                        Publisher = this.sourcePublisherTextBox.Text,
                        AccessedAt = this.sourceAccessedDatePicker.Date.DateTime
                    };
                    break;
                    }
            }

            var isChecked = this.sourceIsLinkCheckBox.IsChecked;
            
            switch (isChecked)
            {
                case null:
                {
                    return;
                }
                case false:
                    source.Link = "";
                    source.Content = await DataManager.FileToBinary(this.storageFile);
                    break;
                default:
                    source.Link = this.sourceLinkTextBox.Text;
                    break;
            }

            this.NewSource = source;
        }

        private void sourceIsLinkCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.linkOptionsGrid.Visibility = Visibility.Collapsed;
            this.uploadOptionsGrid.Visibility = Visibility.Visible;
        }

        private void sourceIsLinkCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.linkOptionsGrid.Visibility = Visibility.Visible;
            this.uploadOptionsGrid.Visibility = Visibility.Collapsed;
        }

        private async void sourceUploadButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            switch (this.sourceIsVideoCheckBox.IsChecked)
            {
                case null:
                {
                    return;
                }
                case false:
                {
                    picker.FileTypeFilter.Add(".pdf");
                    break;
                }
                default:
                {
                    picker.FileTypeFilter.Add(".mp4");
                    break;
                }
            }
            picker.FileTypeFilter.Add(".pdf");
            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                this.storageFile = file;
                this.sourceUploadButton.Content = "Uploading: " + file.Name;
            }
        }

        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            var newAuthor = new User
            {
                Username = "Click to edit"
            };
            this.sourceAuthorsListView.Items?.Add(newAuthor);
        }

        private async void RemoveAuthor_Click(object sender, RoutedEventArgs e)
        {
            var selectedItemIndex = this.sourceAuthorsListView.SelectedIndex;
            if (selectedItemIndex != -1)
            {
                this.sourceAuthorsListView.Items?.RemoveAt(selectedItemIndex);
            }
            else
            {
                var messageDialog = new MessageDialog("Please select an author to remove.");
                await messageDialog.ShowAsync();
            }
        }

        private void sourceAuthorsListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((ListView)sender);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var selectedItemTextBox = (User)textBox.DataContext;
            selectedItemTextBox.Username = textBox.Text;
        }

        #endregion
    }
}