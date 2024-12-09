Imports Microsoft.Office.Interop
Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.Drawing.Drawing2D

Public Class Achievelist

    ' Connection string (make sure to replace with your actual DB connection details)
    Private connectionString As String = "Server=localhost; Database=qcu_clinic; uid=root; password=;"

    ' Method to load data from multiple tables and combine them
    Private Sub loadCombinedData()
        ' Define queries for each table with SELECT * to fetch all columns
        Dim queries As New List(Of String) From {
        "SELECT * FROM student",   ' Add a Type to differentiate the tables
        "SELECT * FROM faculty",
        "SELECT * FROM staff"
    }

        ' Create an empty DataTable to store combined data
        Dim combinedTable As New DataTable()

        Try
            ' Open database connection
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                ' Loop through each query and load the data into the DataTable
                For Each query As String In queries
                    Dim cmd As New MySqlCommand(query, conn)
                    Dim adapter As New MySqlDataAdapter(cmd)

                    ' Fill the DataTable with data from each table
                    adapter.Fill(combinedTable)
                Next
            End Using

            ' Bind the combined data to DataGridView
            DataGridView1.DataSource = combinedTable

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    ' Method to handle form load event
    Private Sub Achievelist_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load combined data when form loads
        loadCombinedData()
    End Sub

    ' Custom form painting (gradient background)
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        ' Define your gradient colors
        Dim color1 As Color = ColorTranslator.FromHtml("#F89B57")
        Dim color2 As Color = ColorTranslator.FromHtml("#FFE4BF")

        ' Create a linear gradient brush
        Using gradientBrush As New LinearGradientBrush(Me.ClientRectangle, color1, color2, 255)
            ' Fill the form with the gradient
            e.Graphics.FillRectangle(gradientBrush, Me.ClientRectangle)
        End Using
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        loadCombinedData()
    End Sub



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
End Class
