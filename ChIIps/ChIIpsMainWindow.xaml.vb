Imports Microsoft.Win32

Class MainWindow

    Dim TransformColl As New TransformGroup

    Dim Elements As New ObjectModel.Collection(Of ElementContainer)
    Dim Wires As New ObjectModel.Collection(Of Wire)

    Public Event ElementRemoved(ByVal Element As Chip.ChIIpsSDK.Chip)
    Public Event ChipMoved()
    Dim ofdCircuit As New OpenFileDialog
    Dim sfdCircuit As New SaveFileDialog

    Public tmpTarget As connectionUI
    Public tmpSource As connectionUI

    Dim ost As Point

    Dim ChipIndex As New Dictionary(Of String, Chip.ChIIpsSDK.Chip)

    Dim CatUIIndex As New Dictionary(Of String, List(Of ElementContainer))

    Dim AutoWireCatalog As New Dictionary(Of String, ElementContainer)

    Private Function Split(ByVal Line As String) As ObjectModel.Collection(Of String)

        Dim Buffer As String = ""
        Dim All As New ObjectModel.Collection(Of String)

        For Each Chr As Char In Line

            If Chr = "," Then
                All.Add(Buffer)
                Buffer = ""
            Else
                Buffer &= Chr
            End If

        Next

        If Buffer.Length <> 0 Then
            All.Add(Buffer)
        End If

        Return All

    End Function

    Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        AddToChooseUI()
        InitSave()
        InitZoom()
    End Sub

    Sub AddToChooseUI()
        For Each Typetje As Type In Windows.Application.ResourceAssembly.GetTypes
            If GetType(Chip.ChIIpsSDK.Chip).IsAssignableFrom(Typetje) Then
                Dim Result As Chip.ChIIpsSDK.Chip
                Dim newContainer As ElementContainer

                Result = Activator.CreateInstance(Typetje)
                newContainer = New ElementContainer(Result, True, Me, Nothing, "")

                ''                stkTronica.Children.Add(newContainer)

                If Not CatUIIndex.Keys.Contains(Result.Classification) Then
                    Dim tmpButton As Button

                    CatUIIndex.Add(Result.Classification, New List(Of ElementContainer))

                    tmpButton = New Button With {.Content = Result.Classification}
                    stkTronica.Children.Add(tmpButton)

                    AddHandler tmpButton.Click, AddressOf OpenComponentList
                End If

                CatUIIndex(Result.Classification).Add(newContainer)

                ChipIndex.Add(Result.Name.ToLower, Result)

                AddHandler newContainer.Choose, AddressOf AddElement

            End If
        Next
    End Sub

    Sub OpenComponentList(ByVal Sender As Object, ByVal E As EventArgs)
        Dim tmpButton As Button = Sender

        stkSpecific.Children.Clear()

        For Each Elm As ElementContainer In CatUIIndex(tmpButton.Content)
            stkSpecific.Children.Add(Elm)
        Next

        brdComp.Visibility = Windows.Visibility.Visible

        brdComp.Margin = New Thickness(Mouse.GetPosition(stkTronica).X, 32, 0, 0)
    End Sub

    Sub InitSave()

        ofdCircuit.Title = "Laad een schakeling"
        ofdCircuit.Filter = "Schakeling|*.schakeling"

        sfdCircuit.Title = "Sla een schakeling op"
        sfdCircuit.Filter = "Schakeling|*.schakeling"

    End Sub

    Sub InitZoom()
        TransformColl.Children.Add(Zoomer)
        TransformColl.Children.Add(Mover)

        brdCircuit.RenderTransform = TransformColl
    End Sub

#Region "Planting"
    Private Sub AddElement(ByVal Element As Chip.ChIIpsSDK.Chip, ByVal Container As ElementContainer, Optional ByVal X As Integer = 0, Optional ByVal Y As Integer = 0, Optional ByVal ID As String = "Unsaved")
        HideSelector()

        Dim Result As Chip.ChIIpsSDK.Chip
        Dim newContainer As ElementContainer

        Result = Activator.CreateInstance(Element.GetType)
        newContainer = New ElementContainer(Result, False, Me, grdCircuit, "")

        grdCircuit.Children.Add(newContainer)
        Elements.Add(newContainer)

        AddHandler newContainer.Choose, AddressOf RemoveElement
        AddHandler newContainer.DupeChoose, AddressOf AddElement

        If Not ID = "Unsaved" Then
            newContainer.ID = ID
            AutoWireCatalog.Add(ID, newContainer)
        End If

        With newContainer
            .Margin = New Thickness(X, Y, 0, 0)
            .HorizontalAlignment = 0
            .VerticalAlignment = 0
        End With
    End Sub
    Private Sub RemoveElement(ByVal Element As Chip.ChIIpsSDK.Chip, ByVal Container As ElementContainer)
        RaiseEvent ElementRemoved(Element)
        grdCircuit.Children.Remove(Container)
        Elements.Remove(Container)
    End Sub
    Public Sub Movement()
        RaiseEvent ChipMoved()
    End Sub
#End Region

#Region "Wiring"
    Public Sub TargetConnectionClick(ByVal Connection As connectionUI)
        tmpTarget = Connection
        If (Not tmpSource Is Nothing) And (Not tmpTarget Is Nothing) Then

            ConnectNow()

        End If
    End Sub
    Public Sub SourceConnectionClick(ByVal Connection As connectionUI)
        tmpSource = Connection
        If (Not tmpSource Is Nothing) And (Not tmpTarget Is Nothing) Then

            ConnectNow()

        End If
    End Sub
    Private Sub RemoveWire(ByVal oldWire As Wire)
        grdCircuit.Children.Remove(oldWire.Line)

        oldWire.Active = False
        Wires.Remove(oldWire)
    End Sub
    Private Sub ConnectNow()
        Dim tmpLine As Line
        tmpLine = New Line
        grdCircuit.Children.Add(tmpLine)

        Try
            Dim GenTrans As GeneralTransform = tmpSource.ellipse.TransformToAncestor(grdCircuit)
            Dim spoint As Point = GenTrans.Transform(New Point(10, 10))

            GenTrans = tmpTarget.ellipse.TransformToAncestor(grdCircuit)
            Dim tpoint As Point = GenTrans.Transform(New Point(10, 10))


            tmpLine.X1 = spoint.X
            tmpLine.X2 = tpoint.X
            tmpLine.Y1 = spoint.Y
            tmpLine.Y2 = tpoint.Y
        Catch ex As Exception

        End Try


        Dim WireConnection As New Wire(tmpSource, tmpTarget, tmpLine, grdCircuit)
        AddHandler ChipMoved, AddressOf WireConnection.AdjustPos
        AddHandler ElementRemoved, AddressOf WireConnection.RemoveIfYours
        AddHandler WireConnection.WireRemoved, AddressOf RemoveWire

        Wires.Add(WireConnection)

        tmpTarget = Nothing
        tmpSource = Nothing
        tmpLine.Stroke = New SolidColorBrush(Colors.Red)
        tmpLine = Nothing
    End Sub
#End Region

#Region "Save, load."

    Private Sub Save() Handles imgSave.MouseUp
        If sfdCircuit.ShowDialog Then

            Dim stsLoad As New Settings

            stsLoad.Filename = sfdCircuit.FileName

            Dim setTemp As Settings.Setting
            Dim elmTemp As ElementContainer

            For i As Integer = 0 To Elements.Count - 1
                elmTemp = Elements(i)
                setTemp = New Settings.Setting("Elements")

                setTemp.Name = "Element" & i
                elmTemp.ID = setTemp.Name

                With setTemp.Values
                    .Add(elmTemp.MyElement.Name)
                    .Add(elmTemp.Margin.Left)
                    .Add(elmTemp.Margin.Top)
                End With

                stsLoad.Add(setTemp)
            Next

            Dim wirTemp As Wire
            Dim intWireCount As Integer

            For i As Integer = 0 To Wires.Count - 1
                wirTemp = Wires(i)

                If wirTemp.Active Then

                    setTemp = New Settings.Setting("Wires")

                    setTemp.Name = "Wire" & intWireCount
                    intWireCount += 1

                    With setTemp.Values
                        .Add(wirTemp.Source.ParentChip.ID)
                        .Add(wirTemp.Target.ParentChip.ID)
                        .Add(wirTemp.Source.GetIndex)
                        .Add(wirTemp.Target.GetIndex)
                    End With

                    stsLoad.Add(setTemp)

                End If

            Next

            stsLoad.SaveFile()

        End If
    End Sub
    Private Sub Load() Handles imgOpen.MouseUp
        If ofdCircuit.ShowDialog Then

            AutoWireCatalog.Clear()

            Dim stsLoad As New Settings

            stsLoad.Loadfile(ofdCircuit.FileName)

            For Each setThing As Settings.Setting In stsLoad.GetSettingsByHeader("elements")

                DeriveChip(setThing)

            Next

            For Each setThing As Settings.Setting In stsLoad.GetSettingsByHeader("wires")

                DeriveWire(setThing)

            Next

        End If
    End Sub
    Sub DeriveChip(ByVal setChip As Settings.Setting)

        Dim Name, ID As String : Name = setChip.Values(1) : ID = setChip.Name
        Dim X, Y As Integer : X = setChip.Values(2) : Y = setChip.Values(3)

        AddElement( _
                ChipIndex(Name), Nothing, _
                X, Y, ID)

    End Sub
    Sub DeriveWire(ByVal setWire As Settings.Setting)

        Dim strOne, strTwo As String
        Dim ixOne, ixTwo As Integer
        Dim One, Two As ElementContainer

        strOne = setWire.Values(1) : strTwo = setWire.Values(2)
        ixOne = setWire.Values(3) : ixTwo = setWire.Values(4)

        One = AutoWireCatalog(strOne)
        Two = AutoWireCatalog(strTwo)

        SourceConnectionClick(One.grdOutputs.Children(ixOne))
        TargetConnectionClick(Two.grdInputs.Children(ixTwo))
    End Sub

    ' Not entirely sure what this down here does but I'll just leave it in =]
    Sub MakeWire(ByVal setThing As Settings.Setting)

    End Sub
    Sub MakeElement(ByVal setThing As Settings.Setting)
        Dim RightElm As Chip.ChIIpsSDK.Chip = Nothing
        For Each elmTemp As ElementContainer In stkTronica.Children
            If elmTemp.MyElement.Name.ToLower = setThing.Values(1) Then
                RightElm = elmTemp.MyElement
            End If
        Next

        If RightElm Is Nothing Then Return

        AddElement(RightElm, Nothing)
    End Sub
#End Region

#Region "Zooming"
    Dim Zoomer As New ScaleTransform
    Dim Mover As New TranslateTransform
    Private Sub sldZoom_ValueChanged(ByVal sender As Object, ByVal e As System.Windows.RoutedPropertyChangedEventArgs(Of Double)) Handles sldZoom.ValueChanged
        Zoomer.ScaleX = sldZoom.Value
        Zoomer.ScaleY = sldZoom.Value
    End Sub
    Private Sub grdCircuit_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        If e.MiddleButton = MouseButtonState.Pressed Then
            ost = e.GetPosition(grdCircuit)
        End If
    End Sub
    Private Sub grdCircuit_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs)
        If e.MiddleButton = MouseButtonState.Pressed Then
            Dim pt As Point = e.GetPosition(grdCircuit)
            Dim opt As Point = pt - ost

            Mover.X += opt.X
            Mover.Y += opt.Y
        End If
    End Sub
    Private Sub grdCircuit_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Input.MouseWheelEventArgs)
        If Windows.Forms.Control.ModifierKeys = Forms.Keys.Control Then
            Zoomer.CenterX = e.GetPosition(brdCircuit).X
            Zoomer.CenterY = e.GetPosition(brdCircuit).Y
            sldZoom.Value += e.Delta / 750
        End If
    End Sub
    Private Sub sldTekengebied_ValueChanged(ByVal sender As Object, ByVal e As System.Windows.RoutedPropertyChangedEventArgs(Of Double)) Handles sldTekengebied.ValueChanged
        brdCircuit.Width = 800 * sldTekengebied.Value
        brdCircuit.Height = 600 * sldTekengebied.Value
    End Sub
#End Region

    Private Sub MainWindow_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles Me.MouseUp
        HideSelector()
    End Sub

    Sub HideSelector()
        If Not Keyboard.Modifiers = ModifierKeys.Shift Then
            brdComp.Visibility = Windows.Visibility.Collapsed
        End If
    End Sub
End Class
