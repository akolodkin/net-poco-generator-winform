Imports System.Runtime.CompilerServices

Module StringExtensions

    <Extension()>
    Public Function ToCSharpName(ByVal aString As String) As String
        Return System.Globalization.CultureInfo.CurrentCulture.TextInfo _
        .ToTitleCase(aString.Replace("-", " ").ToLower()) _
        .Replace(" ", "")
    End Function

End Module