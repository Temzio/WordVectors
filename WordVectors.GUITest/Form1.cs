using System.Reflection;

namespace WordVectors.GUITest
{
    public partial class Form1 : Form
    {
        private Word2VecModel Model { get; set; }
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string wordToTest = textBox1.Text;
            if (Model is null)
            {
                richTextBox2.Text="مشکلی در بارگزاری مدل پیش آمده است.";
                return;
            }
            if (Model.HasWord(wordToTest))
            {
                Console.WriteLine($"Words similar to '{wordToTest}':");
                var similarWords = Model.FindSimilar(wordToTest);
                if (similarWords != null)
                {
                    richTextBox2.Text="";
                    foreach (var (word, similarity) in similarWords)
                    {
                        richTextBox2.Text+=$"  - {word} (شباهت: {similarity:F4})\n";
                    }
                }
            }
            else
            {
                richTextBox2.Text=$"کلمه '{wordToTest}' در این مدل پیدا نشد.";
            }
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                string filePath = openFileDialog1.FileName;
                Model= Word2VecModel.Load(filePath);
            }
            catch (Exception ex)
            {
                richTextBox2.Text="مشکلی در بارگزاری مدل پیش آمده است. \n";
                richTextBox2.Text += ex.ToString();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
