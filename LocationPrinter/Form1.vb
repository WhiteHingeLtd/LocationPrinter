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
            Dim Row as Integer = 1
            Dim Column As Integer = 0
            For each Pair As KeyValuePair(Of string,String) in ListOf27Barcodes
                Column += 1
                Dim TextPoint as New Point
                Dim BarcodePoint as New Point

                If Column = 4
                    Column = 1
                    Row += 1
                End If
                Select Case Column
                    Case 1
                        BarcodePoint.X = 4
                        TextPoint.X = 10
                    Case 2
                        BarcodePoint.X = 69
                        TextPoint.X = 76
                    Case 3
                        BarcodePoint.X = 135
                        TextPoint.X = 142
                        
                End Select
                Select Case Row
                    Case 1
                        BarcodePoint.Y = 27
                        TextPoint.Y = 15
                    Case 2
                        BarcodePoint.Y = 57
                        TextPoint.Y = 45
                    Case 3
                        BarcodePoint.Y = 87
                        TextPoint.Y = 74
                    Case 4
                        BarcodePoint.Y = 116
                        TextPoint.Y = 104
                    Case 5
                        BarcodePoint.Y = 146
                        TextPoint.Y = 133
                    Case 6
                        BarcodePoint.Y = 176
                        TextPoint.Y = 163
                    Case 7
                        BarcodePoint.Y = 205
                        TextPoint.Y = 192
                    Case 8
                        BarcodePoint.Y = 234
                        TextPoint.Y = 221
                    Case 9
                        BarcodePoint.Y = 263
                        TextPoint.Y = 250

                End Select
                Console.WriteLine("Barcode Point " +BarcodePoint.ToString)
                Console.WriteLine("Text Point " + TextPoint.ToString)

                    'e.Graphics.DrawImageUnscaled(barcodes.CreateBarcode(Pair.Value.PadLeft(6, "0"), 40, 2.25),New Point(BarcodePoint.X,BarcodePoint.Y))
                    e.Graphics.DrawImageUnscaled(barcodes.CreateBarcode("qlo"+Pair.Value.replace("qlo","").PadLeft(6,"0"), 40, 2.25),New Point(BarcodePoint.X+1,BarcodePoint.Y))
                    e.Graphics.DrawString(Pair.Key, New Font("Arial", 36,FontStyle.Bold), Black, New RectangleF(TextPoint.X,TextPoint.Y-2,50,11), GoodFormat)
                    e.Graphics.DrawString(Pair.Value.replace("qlo","").TrimStart("0"), New Font("Arial", 9,FontStyle.Bold), Black, New RectangleF(TextPoint.X,BarcodePoint.Y-8,50,11), GoodFormat)

            Next
            'e.Graphics.DrawRectangles(New Pen(Black,0.25), {
            '  New Rectangle(2, 10, 63.5, 29.6),
            '  New Rectangle(68, 10, 63.5, 29.6),
            '  New Rectangle(134, 10, 63.5, 29.6),
            '  New Rectangle(2, 40, 63.5, 29.6),
            '  New Rectangle(68, 40, 63.5, 29.6),
            '  New Rectangle(134, 40, 63.5, 29.6),
            '  New Rectangle(2, 70, 63.5, 29.6),
            '  New Rectangle(68, 70, 63.5, 29.6),
            '  New Rectangle(134, 70, 63.5, 29.6),
            '  New Rectangle(2, 100, 63.5, 29.6),
            '  New Rectangle(68, 100, 63.5, 29.6),
            '  New Rectangle(134, 100, 63.5, 29.6),
            '  New Rectangle(2, 130, 63.5, 29.6),
            '  New Rectangle(68, 130, 63.5, 29.6),
            '  New Rectangle(134, 130, 63.5, 29.6),
            '  New Rectangle(2, 160, 63.5, 29.6),
            '  New Rectangle(68, 160, 63.5, 29.6),
            '  New Rectangle(134, 160, 63.5, 29.6),
            '  New Rectangle(2, 190, 63.5, 29.6),
            '  New Rectangle(68, 190, 63.5, 29.6),
            '  New Rectangle(134, 190, 63.5, 29.6),
            '  New Rectangle(2, 220, 63.5, 29.6),
            '  New Rectangle(68, 220, 63.5, 29.6),
            '  New Rectangle(134, 220, 63.5, 29.6),
            '  New Rectangle(2, 250, 63.5, 29.6),
            '  New Rectangle(68, 250, 63.5, 29.6),
            '  New Rectangle(134, 250, 63.5, 29.6)
            '  })

        Else
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
                        e.Graphics.DrawRectangles(Pens.Black, {
                          New Rectangle(2, 10, 63.5, 29.6),
                          New Rectangle(68, 10, 63.5, 29.6),
                          New Rectangle(134, 10, 63.5, 29.6),
                          New Rectangle(2, 40, 63.5, 29.6),
                          New Rectangle(68, 40, 63.5, 29.6),
                          New Rectangle(134, 40, 63.5, 29.6),
                          New Rectangle(2, 70, 63.5, 29.6),
                          New Rectangle(68, 70, 63.5, 29.6),
                          New Rectangle(134, 70, 63.5, 29.6),
                          New Rectangle(2, 100, 63.5, 29.6),
                          New Rectangle(68, 100, 63.5, 29.6),
                          New Rectangle(134, 100, 63.5, 29.6),
                          New Rectangle(2, 130, 63.5, 29.6),
                          New Rectangle(68, 130, 63.5, 29.6),
                          New Rectangle(134, 130, 63.5, 29.6),
                          New Rectangle(2, 160, 63.5, 29.6),
                          New Rectangle(68, 160, 63.5, 29.6),
                          New Rectangle(134, 160, 63.5, 29.6),
                          New Rectangle(2, 190, 63.5, 29.6),
                          New Rectangle(68, 190, 63.5, 29.6),
                          New Rectangle(134, 190, 63.5, 29.6),
                          New Rectangle(2, 220, 63.5, 29.6),
                          New Rectangle(68, 220, 63.5, 29.6),
                          New Rectangle(134, 220, 63.5, 29.6),
                          New Rectangle(2, 250, 63.5, 29.6),
                          New Rectangle(68, 250, 63.5, 29.6),
                          New Rectangle(134, 250, 63.5, 29.6)
                          })
            

            'X Diff = 66 Y diff = 30
            e.Graphics.DrawString("PAL 99",New Font("Arial", 32,FontStyle.Bold),Black,New RectangleF(10,13,50,11))
            e.Graphics.DrawImageUnscaled(barcodes.CreateBarcode("qlo261171",35,2),New Point(5,28))
            e.Graphics.DrawString("PAL 99",New Font("Arial", 32,FontStyle.Bold),Black,New RectangleF(76,43,50,11))
            e.Graphics.DrawImageUnscaled(barcodes.CreateBarcode("qlo261171",35,2),New Point(71,58))
            e.Graphics.DrawString("PAL 99",New Font("Arial", 32,FontStyle.Bold),Black,New RectangleF(142,73,50,11))
            e.Graphics.DrawImageUnscaled(barcodes.CreateBarcode("qlo261171",35,2),New Point(137,88))
            'Dim textlocation As RectangleF = New RectangleF(0, 0, 57, 19)
            'Dim subtextlocation As RectangleF = New RectangleF(0, 16, 57, 6)
            'Dim Size18B As New Font("Arial", 36.0!, FontStyle.Bold)
            'Dim Size18B As New Font("Arial", 36.0!, FontStyle.Bold) '36 for small text, 48 for large text

            'e.Graphics.DrawImageUnscaled(barcodes.CreateBarcode("qlo" + CurrentID.ToString.PadLeft(6, "0"), 70, 3), New Point(0, 30)) 'Was 13,19
            'e.Graphics.DrawString(CurrentLabel, Size18B, Black, textlocation, GoodFormat)

            'Subtext Black on white
            ''e.Graphics.DrawString(CurrentID + "   " + CurrentType.ToString, Size11, Black, subtextlocation, GoodFormat)

            ''Subtext White on Black
            'e.Graphics.FillRectangle(Brushes.Black, subtextlocation)
            'e.Graphics.DrawString(CurrentID + "   " + CurrentType.ToString, Size11B, White, subtextlocation, GoodFormat)

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
    ''' <summary>
    ''' 
    ''' </summary>
    Private ListOfShelves as New Dictionary(Of String,String)


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Printer.PrintController = (New Printing.StandardPrintController)

        StartButton.Enabled = False
        ListOfShelves.Clear()

        If LocationTB.lines.count>0 then
            For each Line as String in LocationTB.Lines
                Dim Current As String = Line
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
                End If
                'End of cool
                Application.DoEvents()
            Next
            If ListOfShelves.Count > 0
                    CreateSheetOfBarcodes(ListOfShelves)
                End If
        Else
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
        End If
        StartButton.Enabled = True
        End Sub
    Private ListOf27Barcodes as new Dictionary(Of String,String)

    Private Sub CreateSheetOfBarcodes(Shelves As Dictionary(Of String,String))
        Dim CurrentPair as Integer = 0
        ListOf27Barcodes.Clear()
        For each Pair as KeyValuePair(Of String,String) in Shelves
                ListOf27Barcodes.Add(Pair.Key,Pair.Value)

            If ListOf27Barcodes.Count = 27
                PrintingSheet = True
                Printer.Print()
                ListOf27Barcodes.Clear()
                PrintingSheet = False
            End If

        Next
        if ListOf27Barcodes.Count > 0 Then
            PrintingSheet = True
            Printer.Print()
            ListOf27Barcodes.Clear()
            PrintingSheet = False
        End If

    End Sub
    Private Function CreateBarcodeLocation(Column as Integer,Row As Integer) As Point
        Dim ReturnPoint as New Point

        If row = 1
            ReturnPoint.Y = 30
        Else if row = 2
            ReturnPoint.Y = 60
        Else if row = 3    
            ReturnPoint.Y = 90
        Else if row = 4
            ReturnPoint.Y = 120
        Else if row = 5
            ReturnPoint.Y = 150
        Else if row = 6
            ReturnPoint.Y = 180
        Else if row = 7
            ReturnPoint.Y = 210
        Else if row = 8
            ReturnPoint.Y = 240
        Else if row = 9
            ReturnPoint.Y = 270

        End If
        If Column = 1
            ReturnPoint.X = 6
        Else if row = 2
            ReturnPoint.X = 60
        Else if row = 3    
            ReturnPoint.X = 100

        End If


        Return ReturnPoint
    End Function
End Class
