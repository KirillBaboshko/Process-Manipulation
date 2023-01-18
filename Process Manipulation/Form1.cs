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
            //убираем процесс из списка запущенных //приложений
            listBox2.Items.Remove(proc.ProcessName);
            //добавляем процесс в список доступных //приложений
            listBox1.Items.Add(proc.ProcessName);
            //убираем процесс из списка дочерних процессов
            Processes.Remove(proc);
            //уменьшаем счётчик дочерних процессов на 1
            Counter--;
            int index = 0;
            /*меняем текст для главных окон всех дочерних *процессов*/
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
        //импортируем функцию SendMEssage из библиотеки
        //user32.dll
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hwnd, uint Msg, int wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);
        /*список, в котором будут храниться объекты,
        * описывающие дочерние процессы приложения*/
        List<Process> Processes = new List<Process>();
        /*счётчик запущенных процессов*/
        int Counter = 0;
        void LoadAvailableAssemblies()
        {
            //название файла сборки текущего приложения
            string except = new FileInfo(Application.ExecutablePath).Name;
            //получаем название файла без расширения
            except = except.Substring(0, except.IndexOf("."));
            //получаем все *.exe файлы из домашней
            //директории
            string[] files = Directory.GetFiles(Application.StartupPath, "*.exe");
            foreach (var file in files)
            {
                //получаем имя файла
                string fileName = new FileInfo(file).Name;
                /*если имя файла не содержит имени исполняемого
                *файла проекта, то оно добавляется в список*/
                if (fileName.IndexOf(except) == -1)
                {
                    listBox1.Items.Add(fileName);
                }
            }
        }
        void RunProcess(string AssamblyName)
        {
            //запускаем процесс на соновании исполняемого
            //файла
            Process proc = Process.Start(AssamblyName);
            //добавляем процесс в список
            Processes.Add(proc);
            /*проверяем, стал ли созданный процесс дочерним,
             *по отношению к текущему и, если стал, выводим
             *MessageBox*/
            if (Process.GetCurrentProcess().Id == GetParentProcessId(proc.Id))
            {
                MessageBox.Show(proc.ProcessName + " действительно дочерний процесс текущего процесса!");
            }
                /*указываем, что процесс должен генерировать
 *события*/
                 proc.EnableRaisingEvents = true;
            //добавляем обработчик на событие завершения
            //процесса
            proc.Exited += proc_Exited;
            /*устанавливаем новый текст главному окну
             *дочернего процесса*/
            SetChildWindowText(proc.MainWindowHandle, "Child
            process #" + (++Counter));
/*проверяем, запускали ли мы экземпляр такого
 *приложения и, если нет, то добавляем в список
 *запущенных приложений*/
if (!StartedAssemblies.Items.Contains(proc.
ProcessName))
                StartedAssemblies.Items.Add(proc.ProcessName);
            /*убираем приложение из списка доступных
             *приложений*/
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