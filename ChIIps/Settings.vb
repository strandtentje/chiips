Public Class Settings

    Inherits ObjectModel.Collection(Of Setting)

    Public Filename As String
    Dim _Headers As New ObjectModel.Collection(Of String)

    Private Enum SettingType
        Comment = 0
        Setting = 1
        Header = 2
    End Enum

    Private Function GetSettingType(ByVal Line As String) As SettingType
        If Line.Contains(":") And Line.Contains("#") And Not Line.StartsWith("//") Then
            Return SettingType.Setting
        ElseIf Line.StartsWith("[") Then
            Return SettingType.Header
        Else
            Return SettingType.Comment
        End If
    End Function

    Private Function GetSettingPart(ByVal Line As String) As String
        Dim Result As String = ""

        For Each Character As Char In Line
            If (Character = "#") Or (Character = "]") Then
                Exit For
            Else
                Result &= Character
            End If
        Next

        Return Result

    End Function

    Public Sub Loadfile(ByVal File As String)
        Me.Clear()

        If Not IO.File.Exists(File) Then
            IO.File.Create(File)
            Exit Sub
        End If

        Filename = File

        Dim Filereader As New IO.StreamReader(File)
        Dim Line As String
        Dim CurrentSettingType As SettingType
        Dim CurrentHeader As String = ""

        Do Until Filereader.EndOfStream
            Line = Filereader.ReadLine
            CurrentSettingType = GetSettingType(Line)
            If CurrentSettingType = SettingType.Setting Then
                Me.Add(New Setting(CurrentHeader))
                Me.Last.LoadLine(GetSettingPart(Line))
            ElseIf CurrentSettingType = SettingType.Header Then
                CurrentHeader = GetSettingPart(Line.Replace("[", ""))
                If Not Headers.Contains(CurrentHeader) Then Headers.Add(CurrentHeader)
            End If
        Loop

        Filereader.Close()
    End Sub

    Public Sub SaveFile()
        Dim FileWriter As New IO.StreamWriter(Filename)
        Dim CurrentHeader As String = ""

        For Each Setting As Setting In Me

            If (Setting.Header.ToLower <> CurrentHeader.ToLower) Then
                CurrentHeader = Setting.Header.ToLower
                FileWriter.WriteLine("[" & CurrentHeader.ToLower & "]")
            End If

            FileWriter.WriteLine(Setting.GetLine.ToLower)

        Next

        FileWriter.Close()
    End Sub

    Public ReadOnly Property Headers() As ObjectModel.Collection(Of String)
        Get
            Return _Headers
        End Get
    End Property

    Public Function GetItembyName(ByVal Name As String) As Setting

        For Each Setting As Setting In Me
            If Setting.Name.ToLower = Name.ToLower Then
                Return Setting
            End If
        Next

        Return Nothing
    End Function

    Public Overloads Sub SetThing(ByVal Name As String, ByVal Header As String, ByVal Value As String)
        Dim Group As Settings = Nothing
        If Not Header = "" Then
            Group = GetSettingsByHeader(Header)
        End If
        If Group Is Nothing Then Group = Me

        Dim IsolatedSetting As Settings.Setting = Group.GetItembyName(Name)

        If IsolatedSetting Is Nothing Then
            IsolatedSetting = New Settings.Setting(Header)
            IsolatedSetting.Name = Name
        End If

        IsolatedSetting.Values.Clear()
        IsolatedSetting.Values.Add(Value)

        If Me.Contains(IsolatedSetting) Then Me.Remove(IsolatedSetting)
        Me.Add(IsolatedSetting)
    End Sub

    Public Overloads Sub SetThing(ByVal Name As String, ByVal Header As String, ByVal Value As Collection)
        Dim Group As Settings = Nothing
        If Not Header = "" Then
            Group = GetSettingsByHeader(Header)
        End If
        If Group Is Nothing Then Group = Me

        Dim IsolatedSetting As Settings.Setting = Group.GetItembyName(Name)

        If IsolatedSetting Is Nothing Then
            IsolatedSetting = New Settings.Setting(Header)
            IsolatedSetting.Name = Name
        End If

        IsolatedSetting.Values = Value

        If Me.Contains(IsolatedSetting) Then Me.Remove(IsolatedSetting)
        Me.Add(IsolatedSetting)
    End Sub

    Public Function GetSettingsByHeader(ByVal Header As String) As Settings
        Dim FinalIsolation As New Settings

        For Each Setting As Setting In Me
            If Setting.Header.ToLower = Header.ToLower Then
                FinalIsolation.Add(Setting)
            End If
        Next

        FinalIsolation.Filename = Filename

        Return FinalIsolation
    End Function

    Public Sub RemoveSettingByHeader(ByVal Header As String)
        Dim I As Integer

        Do Until I = Me.Count
            If Me(I).Header.ToLower = Header.ToLower Then
                Me.RemoveAt(I)
            Else
                I += 1
            End If
        Loop
    End Sub

    Public Class Setting

        Public Sub New(ByVal GivenHeader As String)
            _Header = GivenHeader
        End Sub

        Dim _Name As String
        Dim _Values As New Collection
        Dim _Header As String

        Function GetLine() As String

            Dim Buffer As String
            Buffer = Name
            Buffer &= ":"

            For Each Value As Object In Values
                Buffer &= Chr(34) & Value.ToString & Chr(34) & ";"
            Next

            Buffer &= "#"

            Return Buffer

        End Function

        Public Property Header() As String
            Set(ByVal value As String)
                _Header = value
            End Set
            Get
                Return _Header
            End Get
        End Property

        Public Property Name() As String
            Set(ByVal value As String)
                _Name = value
            End Set
            Get
                Return _Name
            End Get
        End Property

        Public Property Values() As Collection
            Set(ByVal value As Collection)
                _Values = value
            End Set
            Get
                Return _Values
            End Get
        End Property

        Public Sub LoadLine(ByVal Line As String)
            _Values.Clear()

            Dim Buffer As String = ""
            Dim IsCaptioned As Boolean

            For Each Character As Char In Line
                If Character = Chr(34) Then
                    IsCaptioned = Not IsCaptioned
                    GoTo GoOn
                End If

                If Not IsCaptioned Then
                    If Character = ":" Then
                        _Name = Buffer
                        Buffer = ""
                        GoTo GoOn
                    ElseIf Character = ";" Then
                        If Val(Buffer).ToString.Replace(" ", "") = Buffer Then
                            Values.Add(Int(Buffer))
                        Else
                            Values.Add(Buffer)
                        End If

                        Buffer = ""

                        GoTo GoOn
                    End If
                End If

                Buffer &= Character

GoOn:
            Next

        End Sub

    End Class

End Class
