function addWatchedMovieList(aImdbId, aTitle, aYear, aType) {
    var obj = {
        imdbId: aImdbId,
        title: aTitle,
        year: aYear,
        type: aType
    }
    $.ajax({
        type: "POST",
        url: "/Movie/AddWatchedMovie", // Controller ve Action'ın yolu
        data: obj, // Gönderilecek veri (id)
        dataType: "text",
        success: function () {
            // Başarılı yanıt durumunda yapılacaklar
            console.log("İstek başarılı.");
            setTimeout(function () {
                window.location.reload();
            }, 1000);
            // Eğer gerekliyse, dönen yanıt işlenebilir.
        },
        error: function (error) {
            // Hata durumunda yapılacaklar
            console.log("İstek hatası: " + error);
        }
    });
}
