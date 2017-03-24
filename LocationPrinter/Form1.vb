Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles StartButton.Click

        Printer.PrintController = (New Printing.StandardPrintController)

        StartButton.Enabled = False
        For Each shelf As CheckBox In ShelfGroup.Controls
            If shelf.Checked Then
                shelf.BackColor = Color.PaleGoldenrod
                For Each row As CheckBox In RowGroup.Controls
                    If row.Checked Then
                        row.BackColor = Color.PaleGoldenrod
                        For Each space As CheckBox In SpaceGroup.Controls
                            If space.Checked Then
                                space.BackColor = Color.PaleGoldenrod
                                'ITS HAPPENING
                                Dim Current As String = PrefixBox.Text + shelf.Tag.ToString + "-" + row.Tag.ToString + space.Tag.ToString
                                CurrentLocationLabel.Text = Current
                                Dim response As Object = WHLClasses.MySQL.SelectData("SELECT * FROM whldata.locationreference WHERE loctext='" + Current + "'")
                                If response.GetType = (New ArrayList).GetType Then
                                    Dim list As ArrayList = response
                                    If list.Count = 1 Then
                                        CurrentID = list(0)(0).ToString
                                        CurrentIdLabel.Text = CurrentID
                                        CurrentLabel = Current.ToUpper
                                        CurrentType = list(0)(3)
                                        Printer.DocumentName = Current + " - Shelf label"
                                        Printer.Print()
                                    Else
                                        MsgBox("Could not find an ID for location " + Current)
                                    End If
                                Else
                                    MsgBox("There was an issue with the database. Aborting.")
                                    Exit For
                                    Exit For
                                    Exit For
                                End If
                                'End of cool
                                space.BackColor = SystemColors.Control
                                Application.DoEvents()
                            End If
                        Next
                        row.BackColor = SystemColors.Control
                    End If
                Next
                shelf.BackColor = SystemColors.Control
            End If
        Next
        StartButton.Enabled = True
    End Sub

    Dim CurrentLabel As String = ""
    Dim CurrentID As String = ""
    Dim CurrentType As ShelfType

    Dim Black As New SolidBrush(Color.Black)
    Dim White As New SolidBrush(Color.White)
    Dim textlocation As RectangleF = New RectangleF(2, 3, 72, 19)
    Dim subtextlocation As RectangleF = New RectangleF(2, 22, 72, 6)
    Dim Size11 As New Font("Arial", 11.0!, FontStyle.Regular)
    Dim Size11B As New Font("Arial", 11.0!, FontStyle.Bold)
    Dim Size18 As New Font("Arial", 36.0!, FontStyle.Regular)
    'Dim Size18B As New Font("Arial", 60.0!, FontStyle.Bold) 'Was 48
    Dim barcodes As New WHLClasses.Labels
    Dim GoodFormat As New StringFormat

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        GoodFormat.Alignment = StringAlignment.Center
        GoodFormat.LineAlignment = StringAlignment.Center

    End Sub

    Public Enum ShelfType
        Storage = 0
        Pickable = 1
        Prepack = 2
    End Enum
    Private PrintingSheet as Boolean = False
    Private Sub Printer_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles Printer.PrintPage
        If (PrintingSheet)
        e.Graphics.PageUnit = GraphicsUnit.Millimeter
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            Dim Row as Integer = 0
            Dim Column As Integer = 0
            Dim Size18B As New Font("Arial", 36.0!, FontStyle.Bold)
            For each Pair As KeyValuePair(Of string,String) in ListOf27Barcodes
                Row += 1
                If row = 4
                    Row = 0
                    Column += 1
                End If
                

            Next


        Else
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor

            'Dim textlocation As RectangleF = New RectangleF(0, 0, 57, 19)
            'Dim subtextlocation As RectangleF = New RectangleF(0, 16, 57, 6)
            'Dim Size18B As New Font("Arial", 36.0!, FontStyle.Bold)
            Dim Size18B As New Font("Arial", 36.0!, FontStyle.Bold) '36 for small text, 48 for large text

            e.Graphics.DrawImageUnscaled(barcodes.CreateBarcode("qlo" + CurrentID.ToString.PadLeft(6, "0"), 70, 3), New Point(0, 30)) 'Was 13,19
            e.Graphics.DrawString(CurrentLabel, Size18B, Black, textlocation, GoodFormat)

            ''Subtext Black on white
            'e.Graphics.DrawString(CurrentID + "   " + CurrentType.ToString, Size11, Black, subtextlocation, GoodFormat)

            'Subtext White on Black
            e.Graphics.FillRectangle(Brushes.Black, subtextlocation)
            e.Graphics.DrawString(CurrentID + "   " + CurrentType.ToString, Size11B, White, subtextlocation, GoodFormat)

            'Border for reference - Get rid on the real one
            'e.Graphics.DrawRectangle(New Pen(Color.Black), New Rectangle(0, 0, 57, 33))

            'e.Graphics.DrawRectangles(Pens.Black, {
            '                          New Rectangle(2, 17, 45, 21),
            '                          New Rectangle(50, 17, 45, 21),
            '                          New Rectangle(98, 17, 45, 21),
            '                          New Rectangle(146, 17, 45, 21),
            '                          New Rectangle(2, 38, 45, 21),
            '                          New Rectangle(50, 38, 45, 21),
            '                          New Rectangle(98, 38, 45, 21),
            '                          New Rectangle(146, 38, 45, 21)})
            'e.Graphics.DrawImageUnscaled(barcodes.CreateBarcode("qlo" + CurrentID.ToString.PadLeft(6, "0"), 30, 2), New Point(3, 18))
            'e.Graphics.DrawImageUnscaled(barcodes.CreateBarcode("qlo" + CurrentID.ToString.PadLeft(6, "0"), 30, 3), New Point(99, 18))
            'e.Graphics.DrawImageUnscaled(barcodes.CreateBarcode("qlo" + CurrentID.ToString.PadLeft(6, "0"), 30, 1), New Point(3, 39))

            'e.Graphics.DrawString("TL: 30 height by 2x width." + vbNewLine + "TR: 30 height by 3x width." + vbNewLine + "BL: 30 height by 1x width." + vbNewLine, Size18, Brushes.Black, New Point(4, 61))
        End if


    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim start1, start2, end1, end2, pad1, pad2 As Integer
        start1 = Convert.ToInt32(p1_Start.Text)
        start2 = Convert.ToInt32(p2_start.Text)
        end1 = Convert.ToInt32(p1_end.Text)
        end2 = Convert.ToInt32(p2_end.Text)
        pad1 = Convert.ToInt32(p1_padding.Text)
        pad2 = Convert.ToInt32(p2_padding.Text)

        For count1 As Integer = start1 To end1
            For count2 As Integer = start2 To end2
                Dim Current As String = SourceBox.Text.Replace("{1}", count1.ToString.PadLeft(pad1, "0")).Replace("{2}", count2.ToString.PadLeft(pad2, "0"))
                Currentlabel1.Text = Current
                Dim response As Object = WHLClasses.MySQL.SelectData("SELECT * FROM whldata.locationreference WHERE loctext='" + Current + "'")
                If response.GetType = (New ArrayList).GetType Then

                    Dim list As ArrayList = response
                    If list.Count = 1 Then
                        CurrentID = list(0)(0).ToString
                        currentID1.Text = CurrentID
                        CurrentLabel = Current
                        CurrentType = list(0)(3)
                        Application.DoEvents()
                        Printer.DocumentName = Current + " - Shelf label"
                        Printer.Print()
                    Else
                        MsgBox("Could not find an ID for location " + Current)
                    End If
                Else
                    MsgBox("There was an issue with the database. Aborting.")
                    Exit For
                    Exit For
                    Exit For
                End If
            Next
        Next

    End Sub
    Private NumberOnSheet as Integer = 27
    Private SheetXSize as Integer 
    Private ListOfShelves as Dictionary(Of String,String)

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Printer.PrintController = (New Printing.StandardPrintController)

        StartButton.Enabled = False
        Try
        For Each shelf As CheckBox In ShelfGroup.Controls
            If shelf.Checked Then
                shelf.BackColor = Color.PaleGoldenrod
                For Each row As CheckBox In RowGroup.Controls
                    If row.Checked Then
                        row.BackColor = Color.PaleGoldenrod
                        For Each space As CheckBox In SpaceGroup.Controls
                            If space.Checked Then
                                space.BackColor = Color.PaleGoldenrod
                                'ITS HAPPENING
                                Dim Current As String = PrefixBox.Text + shelf.Tag.ToString + "-" + row.Tag.ToString + space.Tag.ToString
                                CurrentLocationLabel.Text = Current
                                Dim response As Object = WHLClasses.MySQL.SelectData("SELECT * FROM whldata.locationreference WHERE loctext='" + Current + "'")
                                If response.GetType = (New ArrayList).GetType Then
                                    Dim list As ArrayList = response
                                    If list.Count = 1 Then
                                        CurrentID = list(0)(0).ToString
                                        CurrentIdLabel.Text = CurrentID
                                        CurrentLabel = Current.ToUpper
                                        CurrentType = list(0)(3)
                                        Printer.DocumentName = Current + " - Shelf label"
                                        ListOfShelves.Add(Current.ToUpper,"qlo"+CurrentID)
                                        'Printer.Print()
                                    Else
                                        MsgBox("Could not find an ID for location " + Current)
                                    End If
                                Else
                                    MsgBox("There was an issue with the database. Aborting.")
                                    Exit For
                                    Exit For
                                    Exit For
                                End If
                                'End of cool
                                space.BackColor = SystemColors.Control
                                Application.DoEvents()
                            End If
                        Next
                        row.BackColor = SystemColors.Control
                    End If
                Next
                shelf.BackColor = SystemColors.Control
            End If
        Next
        Catch ex As Exception
        Finally
            If ListOfShelves.Count > 0
                CreateSheetOfBarcodes(ListOfShelves)
            End If
        End Try
        StartButton.Enabled = True
        End Sub
    Private ListOf27Barcodes as Dictionary(Of String,String)

    Private Sub CreateSheetOfBarcodes(Shelves As Dictionary(Of String,String))
        Dim CurrentPair as Integer = 0
        ListOf27Barcodes.Clear()
        For each Pair as KeyValuePair(Of String,String) in Shelves
                ListOf27Barcodes.Add(Pair.Key,Pair.Value)

            If ListOf27Barcodes.Count = 27
                Printer.Print()
                ListOf27Barcodes.Clear()
            End If

        Next
    End Sub
End Class
