function removeMovie(aImdbId) {
    var obj = {
        imdbId: aImdbId
    }
    $.ajax({
        type: "POST",
        url: "/Movie/Remove", 
        data: obj, 
        dataType: "text",
        success: function () {
         
            console.log("İstek başarılı.");
            window.location.reload();
        },
        error: function (error) {
            
            console.log("İstek hatası: " + error);
        }
    });
}

