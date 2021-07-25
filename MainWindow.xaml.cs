using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
 
namespace INETEC_Task
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int row;
        public int col;
        public Style primaryStyle;
        public Button b;
        public ToolTip toolTip = new ToolTip();

        public MainWindow()
        {
            InitializeComponent();

            // Properties for the Grid that will contain Buttons
            myGrid.Width = 700;
            myGrid.Height = 700;
            myGrid.VerticalAlignment = VerticalAlignment.Stretch;
            myGrid.HorizontalAlignment = HorizontalAlignment.Stretch;

            // Loading the xml Document to extract the Tubesheet data elements
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Tubesheet.txt");

            // Extracting the list of nodes under the Parent Element '/TubesheetModel/Tubes/Tube'
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/TubesheetModel/Tubes/Tube");

            // Extracting the value of Pitch and Diameter
            string PitchS = xmlDoc.DocumentElement.SelectSingleNode("/TubesheetModel/TubesheetPitch").InnerText;
            float Pitch = float.Parse(PitchS);

            string DiameterS = xmlDoc.DocumentElement.SelectSingleNode("/TubesheetModel/TubesheetDiameter").InnerText;
            float Diameter = float.Parse(PitchS);

            // Defining the Rows and Columns in the Grid
            for (int x = 0; x < 57; x++)
            {
                myGrid.ColumnDefinitions.Add(new ColumnDefinition());
                myGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Populating the Grid with Buttons
            foreach (XmlNode node in nodeList)
            {
                // Creating a new button
                b = new Button();

                // A new Syle object which
                primaryStyle = new Style();

                // Assigning the BackgroundProperty of the style oject based on the Staus retrieved from the xml file
                if (node.SelectSingleNode("Status").InnerText == "Unknown")
                {
                    primaryStyle.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Gray));
                }

                else if (node.SelectSingleNode("Status").InnerText == "Plugged")
                {
                    primaryStyle.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Black));
                }

                else if (node.SelectSingleNode("Status").InnerText == "Critical")
                {
                    primaryStyle.Setters.Add(new Setter(Button.BackgroundProperty, Brushes.Red));
                }

                // Populating other Style Properties of primaryStyle object
                primaryStyle.Setters.Add(new Setter(Button.HeightProperty, 40d));
                primaryStyle.Setters.Add(new Setter(Button.WidthProperty, 40d));
                primaryStyle.Setters.Add(new Setter(MarginProperty, new Thickness(Pitch/30)));

                // Assigning the Button Style to the primiaryStyle Object populated above
                b.Style = primaryStyle;

                // Extracting the Row and Column of the button from the xml file
                string x = node.SelectSingleNode("Row").InnerText;
                string y = node.SelectSingleNode("Column").InnerText;

                // Assigning the extracted Row and Column property to the button
                row = Int32.Parse(x);
                col = Int32.Parse(y);
                Grid.SetRow(b, row);
                Grid.SetColumn(b, col);

                // Associating the triggering events and functiion to the button
                // MouseEnter means if the Curser hovers on the Button
                // MouseLeave means if the Cursor moves away from the button
                b.MouseEnter += CursorOnButton;
                b.MouseLeave += CursonNotOnButton;
               
                // Finally adding the button to the Grid
                myGrid.Children.Add(b);
            }

        }

        // Triggering function for MouseEnter
        void CursorOnButton(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            toolTip.Content = ("Row: " + Grid.GetRow(btn).GetHashCode() + " Col: " + Grid.GetColumn(btn).GetHashCode());
            toolTip.StaysOpen = false;
            toolTip.IsOpen = true;
        }

        // Triggering function for MouseLeave
        void CursonNotOnButton(object sender, MouseEventArgs e)
        {
            toolTip.IsOpen = false;
        }

    }

}
