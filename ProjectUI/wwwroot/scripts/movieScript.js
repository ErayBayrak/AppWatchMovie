function addMovie(aImdbId,aTitle,aYear,aType) {
    var obj = {
        imdbId: aImdbId,
        title: aTitle,
        year: aYear,
        type: aType
    }
    $.ajax({
        type: "POST",
        url: "/Movie/Add", // Controller ve Action'ın yolu
        data: obj, // Gönderilecek veri (id)
        dataType: "text",
        success: function (response) {
            // Başarılı yanıt durumunda yapılacaklar
            console.log("İstek başarılı.");
            // Eğer gerekliyse, dönen yanıt işlenebilir.
        },
        error: function (xhr, status, error) {
            // Hata durumunda yapılacaklar
            console.log("İstek hatası: " + error);
        }
    });
}
