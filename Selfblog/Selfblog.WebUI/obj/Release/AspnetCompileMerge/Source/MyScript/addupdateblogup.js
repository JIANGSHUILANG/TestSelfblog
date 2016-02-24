
function showimage(dom, id) {
    $("#" + id).show();
    $("#" + id).css("height", "500px");
}

function unsave(dom, imageurl) {
    $(dom).next().data("value", 0);
    $(dom).parents('.img-operation-btns').parent().css('height', '0px');
}

function unimage(dom) {
    $(dom).parent().remove();
    $("#photo_id").val("");
}

//保存图片
function onsubmitformimage() {
    $('.img-operation-btns button').click();
    var imgurl = $("#savebase64").val();

    if (imgurl.trim() != "") {
        $.ajax({
            url: "/Base/SaveImage", //当前页面与此api不在同一个域，所以这种调用不行 /api/image/save  Base/SaveImage
            data: {
                base64value: imgurl,
                photo_type: 1
            },
            type: "post",
            success: function (json) {
                if (json == null) {
                    alert("上传失败");
                    return;
                }
                //status = 1, message = "图片上传成功", imageurl = url, image_id = image_id
                var data = $.parseJSON(JSON.parse(json));
                $("#photo_id").val(data.image_id);
                var url = $("#allurl").val() + data.imageurl;
                $(".temp tr").empty();
                $(".temp tr").append('<td style="text-align:center;"><img src="' + url + '" width="236" height="177" class="am-img-thumbnail" onclick="imageclick(this)" /><br /><a href="javascript:;;" onclick="unimage(this)"   data-type="title_image"  data-value="' + url + '" class="am-close am-close-alt am-icon-times"></a></td>');
            }
        })
    }
}