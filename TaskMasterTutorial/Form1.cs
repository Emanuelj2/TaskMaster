using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskMasterTutorial.Model;

namespace TaskMasterTutorial
{
    public partial class Form1 : Form
    {
        private TaskMasterDbContext TaskMasterContext;
        public Form1()
        {
            InitializeComponent();
            TaskMasterContext = new TaskMasterDbContext();
            var statuses = TaskMasterContext.Statuses.ToList();

            foreach (Status stat in statuses) 
            {
                cboStatus.Items.Add(stat);
            }
            refreshData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        //READ
        private void refreshData()
        {
            BindingSource bindingSource = new BindingSource();
            var query = from task in TaskMasterContext.Tasks
                        orderby task.DueDate
                        select new{task.Id, TaskName = task.Name, task.DueDate, StatusName = task.Status.Name};

            bindingSource.DataSource = query.ToList();

            dataGridView1.DataSource = bindingSource;
            dataGridView1.Refresh();
        }
        

        //CREATE
        private void cmdCreateTask_Click(object sender, EventArgs e)
        {
            if (cboStatus.SelectedItem != null && !string.IsNullOrWhiteSpace(txtBox.Text))
            {
                string selectedStatusName = cboStatus.SelectedItem.ToString();
                var status = TaskMasterContext.Statuses.FirstOrDefault(s => s.Name == selectedStatusName);

                if (status != null)
                {
                    var newTask = new Model.Task
                    {
                        Name = txtBox.Text.Trim(),
                        StatusId = status.Id,
                        DueDate = dateTimePicker1.Value
                    };

                    TaskMasterContext.Tasks.Add(newTask);
                    TaskMasterContext.SaveChanges();
                    refreshData();
                }
                else
                {
                    MessageBox.Show("Selected status not found.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a task name and select a status.");
            }

        }

        //DELETE
        private void cmdDeleteTask_Click(object sender, EventArgs e)
        {
            var task = TaskMasterContext.Tasks.Find((int)dataGridView1.SelectedCells[0].Value); // Assuming the first column contains the task ID

            TaskMasterContext.Tasks.Remove(task); // Remove the task from the context
            TaskMasterContext.SaveChanges(); // Save changes to the database
            refreshData(); // Refresh the data grid view to reflect the changes
        }

        //UPDATE
        // UPDATE
        private void cmdUpdateTask_Click(object sender, EventArgs e)
        {
            if (cmdUpdateTask.Text == "Update")
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                    txtBox.Text = selectedRow.Cells[1].Value.ToString();
                    dateTimePicker1.Value = (DateTime)selectedRow.Cells[2].Value;

                    foreach (Status s in cboStatus.Items)
                    {
                        if (s.Name == selectedRow.Cells[3].Value.ToString())
                        {
                            cboStatus.SelectedItem = s;
                            break;
                        }
                    }

                    cmdUpdateTask.Text = "Save";
                }
                else
                {
                    MessageBox.Show("Please select a row to update.");
                }
            }
            else if (cmdUpdateTask.Text == "Save")
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    int taskId = (int)selectedRow.Cells[0].Value;

                    var t = TaskMasterContext.Tasks.Find(taskId);
                    if (t != null)
                    {
                        t.Name = txtBox.Text;
                        t.DueDate = dateTimePicker1.Value;

                        if (cboStatus.SelectedItem is Status selectedStatus)
                        {
                            t.StatusId = selectedStatus.Id;
                        }
                        else
                        {
                            MessageBox.Show("Please select a valid status.");
                            return;
                        }

                        TaskMasterContext.SaveChanges();
                        refreshData();

                        
                    }
                    else
                    {
                        MessageBox.Show("Task not found in the database.");
                    }
                }
                else
                {
                    MessageBox.Show("No row selected to save changes.");
                }
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            // Reset fields
            cmdUpdateTask.Text = "Update";
            txtBox.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Now;
            cboStatus.SelectedIndex = -1;
        }
    }
}
