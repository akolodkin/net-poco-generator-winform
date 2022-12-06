Public Class frmMain
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        txtConnectionString.Text = My.Settings.ConnectionString
    End Sub

    Private Sub btnGenerate_Click(sender As System.Object, e As System.EventArgs) Handles btnGenerate.Click
        Dim CodeText As String = String.Empty
        Dim SchemaTable As New DataTable

        Try
            Call GenerateCode(Me.txtConnectionString.Text, Me.txtQuery.Text, Me.txtObjectName.Text, SchemaTable, CodeText, Me.rbVB.Checked, Nothing)
        Catch Ex As Exception
            MessageBox.Show(Ex.Message)
        End Try

        Me.dgvSchemaColumns.DataSource = SchemaTable
        Me.txtCode.Text = CodeText
    End Sub

    Private Sub GenerateCode(ConnectionString As String, Query As String, ObjectName As String, ByRef SchemaTable As DataTable, ByRef Code As String, ByRef ForVB As Boolean, Optional spParms As ArrayList = Nothing)
        If String.IsNullOrEmpty(ConnectionString) Then _
            Throw New Exception("Database Connection String is Required")

        If String.IsNullOrEmpty(Query) Then _
            Throw New Exception("Query String is Required")

        If String.IsNullOrEmpty(ObjectName) Then _
            Throw New Exception("Code Object Name is Required")

        Dim adoHelper = New ADOHelper()
        Dim Columns = adoHelper.GetFields(ConnectionString, Query, SchemaTable)

        If Not IsNothing(Columns) AndAlso Columns.Count > 0 Then
            Dim codeLines As String()
            If ForVB = True Then
                codeLines = adoHelper.GenerateCodeVB(Columns, ObjectName)
            Else
                codeLines = adoHelper.GenerateCodeCS(Columns, ObjectName)
            End If
            Code = adoHelper.StringArrayToText(codeLines)
        End If
    End Sub

    Private Sub txtCode_Click(sender As System.Object, e As System.EventArgs) Handles txtCode.Click
        Dim text = Me.txtCode.Text
        If String.IsNullOrEmpty(text) = False Then
            Dim dr As DialogResult = MessageBox.Show("Copy Text to Clipboard?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
            If dr = Windows.Forms.DialogResult.Yes Then
                Clipboard.SetText(Me.txtCode.Text)
            End If
        End If
    End Sub

    Private Sub rbStoredProcedure_Click(sender As System.Object, e As System.EventArgs) Handles rbStoredProcedure.Click
        Dim txtSP = GetStoredProcedureText()
        Me.txtQuery.Text = txtSP
    End Sub

    Private Sub FileToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles FileToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Function GetStoredProcedureText() As String
        Dim storedProcedureText = String.Empty
        Dim frmSP As New frmStoredProcedure
        frmSP.ShowDialog()
        If String.IsNullOrEmpty(frmSP.StoredProcedureName) = False Then
            storedProcedureText = frmSP.StoredProcedureName
            If frmSP.Parameters.Count > 0 Then
                For Each s As String In frmSP.Parameters
                    Dim fields = s.Split(vbTab)
                    If (IsNothing(fields) = False) AndAlso (fields.Count = 2) Then
                        storedProcedureText += String.Format(" | `{0}`,`{1}`", fields(0), fields(1))
                    End If
                Next
            End If
        End If
        Return storedProcedureText
    End Function

    Private Sub ClearQueryCodeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearQueryCodeToolStripMenuItem.Click
        Me.txtQuery.Text = String.Empty
        Me.txtCode.Text = String.Empty
    End Sub

    Private Sub ClearAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearAllToolStripMenuItem.Click
        Me.txtQuery.Text = String.Empty
        Me.dgvSchemaColumns.DataSource = Nothing
        Me.txtObjectName.Text = String.Empty
        Me.txtCode.Text = String.Empty
        rbQuery.Checked = True
    End Sub
End Class
