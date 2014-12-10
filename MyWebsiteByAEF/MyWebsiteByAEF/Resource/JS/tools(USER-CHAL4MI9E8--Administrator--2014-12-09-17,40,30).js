$(function () {
    //加载标题
    $("#mainTitle").html(doT.template(mainBiaoTi)({}));
    //音乐盒
    //            $("#amazingaudioplayer-1").data("object").playAudio(); //自动播放
    //            $("#amazingaudioplayer-1").bind("amazingaudioplayer.switched", function (event, data) {
    //                console.log(data);
    //                console.log(data.previous);
    //                console.log(data.current);
    //            });
    /*加载音乐盒列表*/
    $.ajax({
        url: 'NET/index.ashx?method=GetMusicList',
        data: {},
        cache: false,
        type: 'POST',
        dataType: 'text',
        async: false, //同步加载
        success: function (da) {
            if (da == '') { return; }
            $("#Music_ul").append(da);
        }
    });
    //轮播图片高度小于120px;
    $('#slides').slidesjs({
        height: 100,
        play: {
            active: true,
            auto: true,
            interval: 4000,
            swap: true
        }
    });
    /*加载天气*/
    $.post('NET/index.ashx?method=GetWeather', {}, function (da) {
        $("#weather1").html(da);
    });
});
//主页数据根据什么排序
function ShowContent(order, pageSize, pageNumber) {
    $("#succ").show();
    PreOrNextShow(true);
    $.post('NET/index.ashx?method=GetContent', { pageNumber: pageNumber, pageSize: pageSize, order: order }, function (da) {
        $("#succ").hide();
        CheckFY(da, pageSize, pageNumber);
        //                    console.log(da);
        //                    console.log(doT.template(mainTemp)(da));
        $("#contentBody").html(doT.template(mainTemp)(da));
    }, 'json');
}
function enterIn(evt) {
    evt = evt ? evt : (window.event ? window.event : null);
    if (evt.keyCode == 13) {
        $("#go").trigger("click"); //模拟点击
    }
}
//控制分页按钮
function CheckFY(data, pageSize, pageNumber) {
    var total = data.total;
    var rowsCount = pageSize * pageNumber;
    pageNumber<1?1:pageNumber;
    if (rowsCount >= total) {//如果pageNum*pageSize大于总数了，就把下一页置为无效
        $("#xyy").addClass('disabled');
    } else { $("#xyy").removeClass('disabled'); }
    //如果pageNum为1，则上一页无效
    if (pageNumber <= 1) {
        $("#syy").addClass('disabled');
    } else { $("#syy").removeClass('disabled'); }
}
//初始化分页
function InitFY(pageSize, pageNumber) {
    pageSize = 5;
    pageNumber = 1;
}
//是否显示上一页下一页按钮
function PreOrNextShow(boolflag) {
    if (boolflag) {
        $("#fyBtn").show();
    } else {
        $("#fyBtn").hide();
    }
}
//分页-上一页
$("#syy").click(function () {
    pageNumber--;
    ShowContent(order, pageSize, pageNumber);
});
//下一页
$("#xyy").click(function () {
    pageNumber++;
    ShowContent(order, pageSize, pageNumber)
});
//点击链接到详情
function ToXQ(id) {
    $("#succ").show();
    PreOrNextShow(false);
    $.post('NET/index.ashx?method=GetContentXq', { id: id }, function (da) {
        $("#succ").hide();
        //console.log("详情：");
        //console.log(da);
        $("#contentBody").html(doT.template(XQTemplate)(da));
    }, 'json');
}
//得到参数
//        var url = location.search;
//        var Request = new Object();
//        if (url.indexOf("?") != -1) {
//            var str = url.substr(1)  //去掉?号
//            strs = str.toLowerCase();
//            strs = strs.split("&");
//            for (var i = 0; i < strs.length; i++) {
//                Request[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
//            }
//        }
//        var ldid = Request["id"];
//        if (ldid != 1) {
//            window.location = "Resource/LoveDan/LoveDan.html";
//        }