using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;

namespace TwitterApp {
    public partial class Form1:Form {
        public Form1() {
            InitializeComponent();
        }

        protected bool validData;
        string path;
        protected Image image;
        protected Thread getImageThread;

        string AccessToken = " 176673560-mPy3DTkQLmYbsFlWzEWlE0ZJb7QrN9FUAG9Npmvr";
        string TokenSecret = "Z1lXcxaDgd776lt53xUYsQ1IWbxj4yCtgIckNGw3U8C6M";
        string ConsumerKey = "4admW5iocJnyThxZav0MRGjsk";
        string ConsumerSecret = "AClOTGW98i5gfpIWuBdzxEuafbvI4oATLGNRhYfLTKzcz2IvW8";

        private void Form1_Load(Object sender,EventArgs e) {
            
            Auth.SetUserCredentials(ConsumerKey,ConsumerSecret,AccessToken,TokenSecret);
            /*var appCreds = Auth.SetApplicationOnlyCredentials(ConsumerKey,ConsumerSecret);
            Auth.InitializeApplicationOnlyCredentials(appCreds);
            Auth.SetUserCredentials(ConsumerKey,ConsumerSecret,AccessToken,TokenSecret);
            Tweet.PublishTweet("If this sends, I've just finished my first Twitter bot.");*/
        }

        /*private void Form1_DragDrop(Object sender,DragEventArgs e) {
            if(validData) {
                while(getImageThread.IsAlive) {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }
                pictureBox1.Image=image;
            }
        }*/
        protected void LoadImage() {
            image=new Bitmap(path);
        }

        private void Form1_DragEnter(Object sender,DragEventArgs e) {
            string filename;
            validData=GetFileName(out filename,e);
            if(validData) {
                path=filename;
                getImageThread=new Thread(new ThreadStart(LoadImage));
                getImageThread.Start();
                e.Effect=DragDropEffects.Copy;
            }
            else
                e.Effect=DragDropEffects.None;
        }
        private bool GetFileName(out string filename, DragEventArgs e) {
            bool ret = false;
            filename=String.Empty;
            if((e.AllowedEffect &DragDropEffects.Copy)==DragDropEffects.Copy) {
                Array data = ((IDataObject)e.Data).GetData("FileDrop") as Array;
                if(data!=null) {
                    if((data.Length==1)&&(data.GetValue(0) is String)) {
                        filename=((string[])data)[0];
                        string ext = Path.GetExtension(filename).ToLower();
                        if((ext==".jpg")||(ext==".png")||(ext==".bmp")) {
                            ret=true;
                        }
                    }
                }
            }
            return ret;
        }

        private void button1_Click(Object sender,EventArgs e) {
            if(textBox1.Text!=null) {
                try {
                    var tweet = Tweet.PublishTweet(textBox1.Text);
                    MessageBox.Show(tweet.IsTweetPublished.ToString());
                    

                    //Tweet.PublishTweet(textBox1.Text);
                    label1.Text="Tweet Posted";
                    textBox1.Text="Enter Tweet Here...";
                }
                catch(Exception e1) {
                    MessageBox.Show("e1 caught.");
                }
                
            }
            else {
                label1.Text="Tweet cannot be blank!";
            }
        }
            /*if(checkBox1.Checked==true) {// &&checkBox2.Checked==true
                string BlogURL = "";//No Blog URL for now.
                byte[] file = File.ReadAllBytes(path);
                var tweet = Tweet.PublishTweetWithImage(BlogURL,file);
                var imageURL = tweet.Entities.Medias.First().MediaType;
                label1.Text="Tweet Posted with Image";
            }
            else {*/

        private void textBox1_Click(Object sender,EventArgs e) {
            if(textBox1.Text=="Enter Tweet Here...")
                textBox1.Text="";
        }
    }
}