Public Class frmStoredProcedure
    Dim ds As New DataSet
    Dim dt As New DataTable("Parameters")
    Dim colName As New DataColumn("ParmName", Type.GetType("System.String"))
    Dim colValue As New DataColumn("ParmValue", Type.GetType("System.String"))
    Public StoredProcedureName As String
    Public Parameters As New List(Of String)

    Public Sub New()
        InitializeComponent()
        dt.Columns.Add(colName)
        dt.Columns.Add(colValue)
        Me.DataGridView1.DataSource = dt
        Me.DataGridView1.Columns(0).Width = Me.DataGridView1.Width * 0.25
        Me.DataGridView1.Columns(1).Width = Me.DataGridView1.Width * 0.75
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        StoredProcedureName = Me.TextBox1.Text
        For Each dr As DataRow In dt.Rows
            Dim parmLine = dr(0) & vbTab & dr(1)
            Parameters.Add(parmLine)
        Next
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        StoredProcedureName = String.Empty
        Parameters.Clear()
        Me.Close()
    End Sub
End Class