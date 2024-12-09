Imports System.Drawing.Drawing2D
Imports MySql.Data.MySqlClient
Imports Excel = Microsoft.Office.Interop.Excel
' Import Excel Interop namespace

Public Class List

    ' Method to load student data into DataGridView1
    Private Sub loadstudentData()
        'Connection String
        Dim connectionString As String = "Server=localhost; Database = qcu_clinic; uid=root; password=;"
        Dim query As String = "SELECT * FROM student"

        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                'Command and Adapter
                Dim cmd As New MySqlCommand(query, conn)
                Dim adapter As New MySqlDataAdapter(cmd)

                'Fill table
                Dim table As New DataTable()
                adapter.Fill(table)

                'Bind to DataGridView
                DataGridView1.DataSource = table

            End Using
        Catch ex As Exception
            MessageBox.Show("Error:" & ex.Message)
        End Try
    End Sub

    ' Method to load faculty data into DataGridView2
    Private Sub loadFacultyData()
        'Connection String
        Dim connectionString As String = "Server=localhost; Database = qcu_clinic; uid=root; password=;"
        Dim query As String = "SELECT * FROM faculty"

        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                'Command and Adapter
                Dim cmd As New MySqlCommand(query, conn)
                Dim adapter As New MySqlDataAdapter(cmd)

                'Fill table
                Dim table As New DataTable()
                adapter.Fill(table)

                'Bind to DataGridView
                DataGridView2.DataSource = table

            End Using
        Catch ex As Exception
            MessageBox.Show("Error:" & ex.Message)
        End Try
    End Sub

    ' Method to load staff data into DataGridView3
    Private Sub loadStaffData()
        'Connection String
        Dim connectionString As String = "Server=localhost; Database = qcu_clinic; uid=root; password=;"
        Dim query As String = "SELECT * FROM staff"

        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                'Command and Adapter
                Dim cmd As New MySqlCommand(query, conn)
                Dim adapter As New MySqlDataAdapter(cmd)

                'Fill table
                Dim table As New DataTable()
                adapter.Fill(table)

                'Bind to DataGridView
                DataGridView3.DataSource = table

            End Using
        Catch ex As Exception
            MessageBox.Show("Error:" & ex.Message)
        End Try
    End Sub

    ' Method to handle form load event
    Public Sub List_Load_1(sender As Object, e As EventArgs) Handles MyBase.Load
        loadstudentData()
        loadFacultyData()
        loadStaffData()
    End Sub

    ' Method to handle custom form painting (gradient background)
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        ' Define your colors
        Dim color1 As Color = ColorTranslator.FromHtml("#F89B57")
        Dim color2 As Color = ColorTranslator.FromHtml("#FFE4BF")

        ' Create a linear gradient brush
        Using gradientBrush As New LinearGradientBrush(Me.ClientRectangle, color1, color2, 255)
            ' Fill the form with the gradient
            e.Graphics.FillRectangle(gradientBrush, Me.ClientRectangle)
        End Using ' Automatically disposes the brush
    End Sub

    ' Method to handle button click to export data to Excel
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ' Export data from DataGridView1 to Excel
        ExportToExcel(DataGridView1)

    End Sub

    ' Method to export DataGridView data to an Excel file
    Private Sub ExportToExcel(dgv As DataGridView)
        ' Create a new instance of Excel application
        Dim excelApp As New Excel.Application()
        excelApp.Visible = True  ' Make Excel visible

        ' Add a new workbook
        Dim workbook As Excel.Workbook = excelApp.Workbooks.Add()
        Dim worksheet As Excel.Worksheet = workbook.Sheets(1)

        ' Adding column headers to Excel
        For col As Integer = 0 To dgv.Columns.Count - 1
            worksheet.Cells(1, col + 1) = dgv.Columns(col).HeaderText
        Next

        ' Adding rows of data to Excel
        For row As Integer = 0 To dgv.Rows.Count - 1
            For col As Integer = 0 To dgv.Columns.Count - 1
                If dgv.Rows(row).Cells(col).Value IsNot Nothing Then
                    worksheet.Cells(row + 2, col + 1) = dgv.Rows(row).Cells(col).Value.ToString()
                End If
            Next
        Next

        ' Optional: Show the SaveFileDialog to let the user choose where to save the Excel file
        Dim saveDialog As New SaveFileDialog()
        saveDialog.Filter = "Excel Files|*.xlsx"
        If saveDialog.ShowDialog() = DialogResult.OK Then
            ' Save the workbook at the chosen location
            workbook.SaveAs(saveDialog.FileName)
            MessageBox.Show("Data exported successfully to " & saveDialog.FileName)
        End If

        ' Optionally: Close the workbook and quit Excel after export
        ' workbook.Close()
        ' excelApp.Quit()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        loadstudentData()
        loadFacultyData()
        loadStaffData()

    End Sub
End Class
