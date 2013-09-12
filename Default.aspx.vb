Imports System.ServiceModel.Syndication
Imports System.Xml
Imports System.Text
Imports System.Data
Imports Common

Partial Class _Default
    Inherits System.Web.UI.Page


    Protected Sub btnGetFeed_Click(sender As Object, e As EventArgs) Handles btnGetFeed.Click

        Dim dtExisting As DataTable = LoadDataTable()
        Dim dtRSS As DataTable = GetRSSTable()
        Dim feed As SyndicationFeed
        Dim MaxDate As Date = Date.MinValue
        Dim iCount As Integer = 0

        For Each dr As DataRow In dtExisting.Rows
            If CDate(dr("Date")) > MaxDate Then MaxDate = CDate(dr("Date"))
        Next

        iCount = dtExisting.Rows.Count
        Dim items As New List(Of FeedItem)

        If dtRSS.Rows.Count > 0 Then
            For Each RSSdr As DataRow In dtRSS.Rows
                If Not RSSdr("URL").Equals(Convert.DBNull) AndAlso RSSdr("URL") <> "" Then
                    If RSSdr("URL").ToString.Contains("arcamax.com") Then
                        'If RSSdr("URL").ToString.Contains("cgi-bin/news") Then
                        '    Dim stest = ""
                        '    feed = SyndicationFeed.Load(XmlReader.Create((RSSdr("URL"))))

                        'Else
                        Dim xr As XmlReader = XmlReader.Create(RSSdr("URL"))
                        Dim item As New FeedItem
                        While xr.Read()
                            If xr.Value.Contains(" - ArcaMax Publishing") Then
                                item = New FeedItem
                                item.Title = xr.Value.Replace(" - ArcaMax Publishing", "")
                            ElseIf xr.Value.Contains("<img src=") Then
                                '
                                item.Source = RSSdr("Name")
                                item.Processed = False
                                item.PublishedDate = Date.Now
                                item.Summary = xr.Value.Substring(xr.Value.IndexOf("<img src=""") + 10)
                                item.Summary = item.Summary.Substring(0, item.Summary.IndexOf(""""))
                                'item.Summary = xr.Value.Substring(xr.Value.IndexOf("<img src=""") + 10, xr.Value.IndexOf("""", xr.Value.IndexOf("<img src=""") - 10))
                                item.URL = xr.Value.Substring(xr.Value.IndexOf("<a href=""") + 9)
                                item.URL = item.URL.Substring(0, item.URL.IndexOf(""""))
                                'item.URL = xr.Value.Substring(9, xr.Value.IndexOf("""", 9) - 9)
                                'item.Title = RSSdr("Name")
                                items.Add(item)
                            End If
                        End While
                        'End If


                    Else
                        feed = SyndicationFeed.Load(XmlReader.Create((RSSdr("URL"))))

                        For Each item As SyndicationItem In feed.Items
                            Dim myItem As FeedItem
                            myItem.Source = RSSdr("Name")
                            If item.Id.ToLower.Contains("gocomics") Then
                                myItem.URL = item.Id
                            ElseIf item.Links.Count > 0 Then
                                myItem.URL = item.Links(0).Uri.AbsoluteUri
                            Else
                                myItem.URL = ""
                            End If
                            myItem.PublishedDate = If(item.PublishDate.Year > 2010, item.PublishDate.DateTime, Date.Now)
                            myItem.Processed = False
                            myItem.Summary = item.Summary.Text
                            myItem.Title = item.Title.Text
                            items.Add(myItem)
                        Next

                    End If

                    For Each item As FeedItem In items
                        Dim bSkip As Boolean = False

                        For Each dr As DataRow In dtExisting.Rows
                            If Not dr("URL").Equals(Convert.DBNull) AndAlso dr("URL").ToString = item.URL Then
                                bSkip = True
                                Exit For
                            End If
                        Next
                        If Not bSkip Then
                            Dim dr As DataRow = dtExisting.NewRow()
                            dr("Source") = item.Source
                            dr("URL") = item.URL
                            dr("Title") = item.Title
                            dr("Summary") = item.Summary
                            dr("Date") = item.PublishedDate
                            dr("Processed") = item.Processed
                            dtExisting.Rows.Add(dr)
                        End If
                    Next
                End If

            Next
        End If
        iCount = dtExisting.Rows.Count - iCount
        lblStatus.Text = "Checked " & dtRSS.Rows.Count.ToString & " feeds. Adding " & iCount.ToString & " new items."
        SaveDataTable(dtExisting)
    End Sub

    Protected Sub btnGetImages_Click(sender As Object, e As EventArgs) Handles btnGetImages.Click
        Dim dtExisting As DataTable = LoadDataTable()
        Dim counter As Integer = 0
        Dim done As Integer = 0

        For Each dr As DataRow In From row In dtExisting Where row("Processed") = False
            If dr("Summary").Equals(Convert.DBNull) OrElse dr("Summary").ToString.Length = 0 Then Continue For
            If dr("Summary").ToString.ToLower.StartsWith("http") AndAlso dr("Summary").ToString.ToLower.EndsWith(".gif") OrElse _
                dr("Summary").ToString.ToLower.StartsWith("http") AndAlso dr("Summary").ToString.ToLower.EndsWith(".jpg") Then
                'GetImageFromURL(dr("URL"))
                dr("Processed") = DownloadImage(dr("Summary"))
            ElseIf dr("URL").ToString.ToLower.Contains("gocomics.com") Then
                Dim webClient As System.Net.WebClient = New System.Net.WebClient()
                Dim result As String = webClient.DownloadString(dr("URL"))
                Dim imageURL
                If result.IndexOf("<img alt=""Collectible Print of") > 0 Then
                    imageURL = result.Substring(result.IndexOf("<img alt=""Collectible Print of"))
                    imageURL = imageURL.Substring(imageURL.IndexOf("src=""") + 5)
                    imageURL = imageURL.Substring(0, imageURL.IndexOf(""""))
                Else
                    imageURL = result.Substring(result.IndexOf("http://cdn.svcs.c2.uclick.com/c2/"))
                    imageURL = imageURL.Substring(0, imageURL.IndexOf(""""))
                End If
                dr("Processed") = DownloadImage(imageURL)
            End If
            If dr("Processed") = True Then done += 1
            counter += 1

        Next
        lblStatus.Text = "Downloaded " & done.ToString & " images out of " & counter.ToString
        SaveDataTable(dtExisting)
    End Sub
End Class
