(function () {
  // var element = $('#username');
  // element.text('Ashley Zhuan');

  // var main = $('#main');
  // main.on('mouseenter', function () {
  //   main.style = 'background-color: #888';
  // });
  // //var main = document.getElementById('main');
  // //main.onmouseenter = function () {};
  // main.on('mouseleave', function () {
  //   main.style = '';
  // });

  // var multiEles = $("ul.menu li a");
  // multiEles.on('click', function(){
  //   var me = $(this);
  //   alert(me.text());
  // })
    var $sidebarAndWrapper = $('#sidebar, #wrapper');
    var $icon = $("#sidebarToggle i.fa");
  $('#sidebarToggle').on('click', function () {
      $sidebarAndWrapper.toggleClass('hide-sidebar');
      if ($sidebarAndWrapper.hasClass('hide-sidebar')) {
          $icon.removeClass("fa-angle-left");
          $icon.addClass("fa-angle-right");
      } else {
          $icon.removeClass("fa-angle-right");
          $icon.addClass("fa-angle-left");
      }
  });
})();
