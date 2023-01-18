using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Management;
using System;


namespace Process_Manipulation
{

    public partial class Form1 : Form
    {
        void SetChildWindowText(IntPtr Handle, string text)
        {
            SendMessage(Handle, WM_SETTEXT, 0, text);
        }
        void proc_Exited(object sender, EventArgs e)
        {
            Process proc = sender as Process;
            //������� ������� �� ������ ���������� //����������
            listBox2.Items.Remove(proc.ProcessName);
            //��������� ������� � ������ ��������� //����������
            listBox1.Items.Add(proc.ProcessName);
            //������� ������� �� ������ �������� ���������
            Processes.Remove(proc);
            //��������� ������� �������� ��������� �� 1
            Counter--;
            int index = 0;
            /*������ ����� ��� ������� ���� ���� �������� *���������*/
            foreach (var p in Processes)
                SetChildWindowText(p.MainWindowHandle, "Child process #" + ++index);
        }
        int GetParentProcessId(int Id)
        {
            int parentId = 0;
            using (ManagementObject obj = new ManagementObject("win32 _ process.handle=" +
             Id.ToString()))
            {
                obj.Get();
                parentId = Convert.
                 ToInt32(obj["ParentProcessId"]);
            }
            return parentId;
        }
        const uint WM_SETTEXT = 0x0C;
        //����������� ������� SendMEssage �� ����������
        //user32.dll
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hwnd, uint Msg, int wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);
        /*������, � ������� ����� ��������� �������,
        * ����������� �������� �������� ����������*/
        List<Process> Processes = new List<Process>();
        /*������� ���������� ���������*/
        int Counter = 0;
        void LoadAvailableAssemblies()
        {
            //�������� ����� ������ �������� ����������
            string except = new FileInfo(Application.ExecutablePath).Name;
            //�������� �������� ����� ��� ����������
            except = except.Substring(0, except.IndexOf("."));
            //�������� ��� *.exe ����� �� ��������
            //����������
            string[] files = Directory.GetFiles(Application.StartupPath, "*.exe");
            foreach (var file in files)
            {
                //�������� ��� �����
                string fileName = new FileInfo(file).Name;
                /*���� ��� ����� �� �������� ����� ������������
                *����� �������, �� ��� ����������� � ������*/
                if (fileName.IndexOf(except) == -1)
                {
                    listBox1.Items.Add(fileName);
                }
            }
        }
        void RunProcess(string AssamblyName)
        {
            //��������� ������� �� ��������� ������������
            //�����
            Process proc = Process.Start(AssamblyName);
            //��������� ������� � ������
            Processes.Add(proc);
            /*���������, ���� �� ��������� ������� ��������,
             *�� ��������� � �������� �, ���� ����, �������
             *MessageBox*/
            if (Process.GetCurrentProcess().Id == GetParentProcessId(proc.Id))
            {
                MessageBox.Show(proc.ProcessName + " ������������� �������� ������� �������� ��������!");
            }
                /*���������, ��� ������� ������ ������������
 *�������*/
                 proc.EnableRaisingEvents = true;
            //��������� ���������� �� ������� ����������
            //��������
            proc.Exited += proc_Exited;
            /*������������� ����� ����� �������� ����
             *��������� ��������*/
            SetChildWindowText(proc.MainWindowHandle, "Child
            process #" + (++Counter));
/*���������, ��������� �� �� ��������� ������
 *���������� �, ���� ���, �� ��������� � ������
 *���������� ����������*/
if (!StartedAssemblies.Items.Contains(proc.
ProcessName))
                StartedAssemblies.Items.Add(proc.ProcessName);
            /*������� ���������� �� ������ ���������
             *����������*/
            AvailableAssemblies.Items.
            Remove(AvailableAssemblies.SelectedItem);
        }
        public Form1()
        {
            InitializeComponent();
            LoadAvailableAssemblies();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}