

(function($){

//
//	--------------------------------------------------------------------------
//	---------  						TOOL TIP						 ---------
//	--------------------------------------------------------------------------	
//	USAGE:
//	
//		$(<element>).toolTip(tooltip);
//

	$.fn.toolTip = function(msg) {
		var helpID;
		$(this).mouseenter(function () {
			var _left = $(this).position().left + 20;
			var _top = $(this).position().top + 20;
			helpID = $(this).attr('id') + '_hlp';
			$(this).css('cursor','inherit');
			$('body').append("<div id='" + helpID + "' class='tooltip_generated' style='top:" + _top + "px; left:" + _left + "px; position:absolute !important;'>" + msg + "</div>");
			$('#' + helpID).fadeIn('slow');
			$(this).mousemove(function(e) {
				$('.tooltip_generated').css({
					'top':(e.pageY + 10) + 'px',
					'left':(e.pageX + 20) + 'px',
				});
			});
		}).mouseleave(function () {
			$('#' + helpID).remove()
		});	
	};


//
//	--------------------------------------------------------------------------
//	---------  					HOVER ANIMATE						 ---------
//	--------------------------------------------------------------------------	
//	USAGE:
//	
//		$(<element>).hoverAnimate(tooltip,[{settings*}]);
//		* settings:{
//			bgColor,
//			color,
//			hBgColor,		-- set bg color when hovered
//			hColor,			-- set text color when hovered
//			duration,		-- animation duration
//			theme			-- [blue, red, green, black] / when set to default: custom settings will be applied
//		}

	$.fn.hoverAnimate = function(o) {
		
		var defaults = {
			bgColor:'#E8E8E8',
			color:'#333',
			hBgColor:'#BFBFBF',
			hColor:'#FFF',
			duration:300,
			theme:'default'
		};
		
		var bgColor = defaults.bgColor;
		var color = defaults.color;
		var hBgColor = defaults.hBgColor;
		var hColor = defaults.hColor;
		var duration = defaults.duration;
		var theme = defaults.theme;
		
		var settings = $.extend({}, defaults, o);
		
		if (o != null) {
			if (o.settings != null) {
				if (o.settings.bgColor != null) { bgColor = o.settings.bgColor; }
				if (o.settings.color != null) { color = o.settings.color; }
				if (o.settings.hBgColor != null) { hBgColor = o.settings.hBgColor; }
				if (o.settings.hColor != null) { hColor = o.settings.hColor; }
				if (o.settings.duration != null) { duration = o.settings.duration; }
				if (o.settings.theme != null) { theme = o.settings.theme; }				
			}
		}
		
		switch(theme) {
			case 'blue':
				bgColor = '#87b5d6';
				color = '#333';
				hBgColor = '#5894BF';
				hColor = '#FFF';
			break;
			case 'red':
				bgColor = '#D68787';
				color = '#333';
				hBgColor = '#BF5858';
				hColor = '#FFF';			
			break;
			case 'green':
				bgColor = '#87D695';
				color = '#333';
				hBgColor = '#58BF6B';
				hColor = '#FFF';			
			break;
			case 'black':
				bgColor = '#616161';
				color = '#FFF';
				hBgColor = '#1F1F1F';
				hColor = '#FFF';			
			break;
			default:break;
		}
		
		$(this).css({
			'background-color':bgColor,
			'color':color
		});
		
	    $(this).hover(function () {
            $(this).animate({ backgroundColor: hBgColor, color: hColor }, duration);
        },
        function () {
            $(this).animate({ backgroundColor: bgColor, color: color }, 0);
        });
	};


//
//	--------------------------------------------------------------------------
//	---------					LOADING PANEL						 ---------
//	--------------------------------------------------------------------------
//	USAGE:
//	
//		var loads;									-- VARIABLE TO STORE THE LOADING ELEMENT
//		
//		loads = $(<element>).loading([{settings*}]);
//		* settings:{
//			text,				-- text to display while loading
//		}
//		
//		loads.hideLoading();						-- CLEAR LOADING PANEL
//		

	$.fn.loading = function(o){
		this.hideLoading = function() {
			$(this).children('#loader_screen_generated').remove();
		};		
		
		if ($(this).children('#loader_screen_generated').css('display') == 'block') {return this;}
		
		var defaults = {
			text:''
		};
		var text = defaults.text;
		
		var settings = $.extend({}, defaults, o);
		if (o != null) {
			if (o.settings != null) {
				if (o.settings.text != null) { text = o.settings.text; }
			}
		}

		var _height = $(this).height();
		var _width = $(this).width();
		var _left = $(this).position().left;
		var _top = $(this).position().top;
		var _textPos = (_height / 10) * 3;
	
		var loadingscreen = '<div id="loader_screen_generated" style="width:' + _width + 'px; height:' + _height + 'px; top: ' + _top + 
            'px; ' + '"><div style="padding-top:' + _textPos + 'px;">' + text + '</div>' +
			'</div>';
		
		
		$(this).append(loadingscreen);
		
		var cl = new CanvasLoader('loader_screen_generated');
		//cl.setColor('#00540a'); // default is '#000000'
		cl.setColor(rgb2hex($('#loader_screen_generated').css('color'))); // default is '#000000'
		cl.setShape('roundRect'); // default is 'oval'
		cl.setDiameter(46); // default is 40
		cl.setDensity(53); // default is 40
		cl.setRange(0.6); // default is 1.3
		cl.setSpeed(3); // default is 2
		cl.setFPS(25); // default is 24
		cl.show(); // Hidden by default
		$('#loader_screen_generated').click();
		return this;
	};
	

//
//	--------------------------------------------------------------------------
//	---------					MESSAGE BOX							 ---------
//	--------------------------------------------------------------------------
//	USAGE:
//	
//		jAlertBox(message,[title],[callback**]);
//		
//		jConfirmBox(message,[title],[callback**]);
//		
//		** function() { [javascript code] }
//

	$.jAlerts = {

		boxInnitialTop: 0,

		alertBox: function(message, title, callback) {
			if (title == null) title = 'Alert';
			if (message.trim().length == 0) message = "&nbsp;";
			$.jAlerts._show(title, message, 'alert', function(result) {
				if (callback) callback(result);
			});
		},
		
		confirmBox: function(message, title, callback) {
			if (title == null) title = 'Confirm';
			if (message.trim().length == 0) message = "&nbsp;";			
			$.jAlerts._show(title, message, 'confirm', function(result) {
				if (callback) callback(result);
			});
		},

		_show: function(title, msg, type, callback) {

			$.jAlerts._hide();
			
			var _body = $(window);
			var _docBody = $('body');
			var _height = _docBody.height() > _body.height() ? _docBody.height() : _body.height();
			var _width = _docBody.width() > _body.width() ? _docBody.width() : _body.width();
		
			$('body').append('<div id="bg_box_generated"></div>');
			$('#bg_box_generated').css({
				'height': _height +'px',
				'width': _width + 'px'
			});
			
			$('body').append(
				'<div id="msg_box_container_generated">' +
					'<div id="msg_box_title_generated"></div>' +
					'<div id="msg_box_content_generated">' +
						'<div id="msg_panel_generated"></div>' +
					'</div>' +
					'<div id="button_container_generated"></div>' +
				'</div>');

			switch(type) {
				case 'alert':
					$('#button_container_generated').append('<a class="box_button_generated" id="btnOk_generated">OK</a>');
					$('#btnOk_generated').click(function(){
						$.jAlerts._hide();
						callback(true);
					});
				break;
				case 'confirm':
					$('#button_container_generated').append(
						'<a class="box_button_generated" id="btnYes_generated">YES</a>' +
						'&nbsp;&nbsp;' +
						'<a class="box_button_generated" id="btnNo_generated">NO</a>'
					);			
					$('#btnYes_generated').click(function(){
						$.jAlerts._hide();
						callback(true);
					});
					$('#btnNo_generated').click(function(){
						$.jAlerts._hide();
						callback(false);
					});					
				break;
			}
			
			$('#msg_box_title_generated').append('<div>' + title + '<div>');
			$('#msg_panel_generated').append(msg);
			

			try {
				$('#msg_box_container_generated').draggable({ handle: $('#msg_box_title_generated')});
				$('#msg_box_title_generated').css('cursor','move');
			}
			catch(e) {}


			$.jAlerts._positionBox();

			$(window).scroll(function(){
				$('#msg_box_container_generated').animate(
						{ top: ($(window).scrollTop() + $.jAlerts.boxInnitialTop) + "px" }, 
						{ queue: false, duration: 500 }
				);
			});
		},
		
		_positionBox: function() {
			var _body = $(window);
			var box = $('#msg_box_container_generated');
			var _top = (_body.height() / 2) - (box.height() / 2);
			var _left = (_body.width() / 2) - (box.width() / 2);
			$('#msg_box_container_generated').css({
				'top': _top + 'px',
				'left': _left + 'px'				
			})
			$.jAlerts.boxInnitialTop = _top;
		},
		
		_hide: function(){
			$('#bg_box_generated').remove();
			$('#msg_box_container_generated').remove();			
		}
	
	}

	// SHORT-CUT FUNCTIONS
	
	jAlertBox = function(message, title, callback) {
		$.jAlerts.alertBox(message, title, callback);
	}

	jConfirmBox = function(message, title, callback) {
		return $.jAlerts.confirmBox(message, title, callback);
	}
	
	
})($);





function validateEmail(email) { 
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
} 

function rgb2hex(rgb) {
     if (  rgb.search("rgb") == -1 ) {
          return rgb;
     } else {
          rgb = rgb.match(/^rgba?\((\d+),\s*(\d+),\s*(\d+)(?:,\s*(\d+))?\)$/);
          function hex(x) {
               return ("0" + parseInt(x).toString(16)).slice(-2);
          }
          return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]); 
     }
}





/* HEART CODE CANVAS LOADER */

(function(w){var k=function(b,c){typeof c=="undefined"&&(c={});this.init(b,c)},a=k.prototype,o,p=["canvas","vml"],f=["oval","spiral","square","rect","roundRect"],x=/^\#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$/,v=navigator.appVersion.indexOf("MSIE")!==-1&&parseFloat(navigator.appVersion.split("MSIE")[1])===8?true:false,y=!!document.createElement("canvas").getContext,q=true,n=function(b,c,a){var b=document.createElement(b),d;for(d in a)b[d]=a[d];typeof c!=="undefined"&&c.appendChild(b);return b},m=function(b,
c){for(var a in c)b.style[a]=c[a];return b},t=function(b,c){for(var a in c)b.setAttribute(a,c[a]);return b},u=function(b,c,a,d){b.save();b.translate(c,a);b.rotate(d);b.translate(-c,-a);b.beginPath()};a.init=function(b,c){if(typeof c.safeVML==="boolean")q=c.safeVML;try{this.mum=document.getElementById(b)!==void 0?document.getElementById(b):document.body}catch(a){this.mum=document.body}c.id=typeof c.id!=="undefined"?c.id:"canvasLoader";this.cont=n("div",this.mum,{id:c.id});if(y)o=p[0],this.can=n("canvas",
this.cont),this.con=this.can.getContext("2d"),this.cCan=m(n("canvas",this.cont),{display:"none"}),this.cCon=this.cCan.getContext("2d");else{o=p[1];if(typeof k.vmlSheet==="undefined"){document.getElementsByTagName("head")[0].appendChild(n("style"));k.vmlSheet=document.styleSheets[document.styleSheets.length-1];var d=["group","oval","roundrect","fill"],e;for(e in d)k.vmlSheet.addRule(d[e],"behavior:url(#default#VML); position:absolute;")}this.vml=n("group",this.cont)}this.setColor(this.color);this.draw();
m(this.cont,{display:"none"})};a.cont={};a.can={};a.con={};a.cCan={};a.cCon={};a.timer={};a.activeId=0;a.diameter=40;a.setDiameter=function(b){this.diameter=Math.round(Math.abs(b));this.redraw()};a.getDiameter=function(){return this.diameter};a.cRGB={};a.color="#000000";a.setColor=function(b){this.color=x.test(b)?b:"#000000";this.cRGB=this.getRGB(this.color);this.redraw()};a.getColor=function(){return this.color};a.shape=f[0];a.setShape=function(b){for(var c in f)if(b===f[c]){this.shape=b;this.redraw();
break}};a.getShape=function(){return this.shape};a.density=40;a.setDensity=function(b){this.density=q&&o===p[1]?Math.round(Math.abs(b))<=40?Math.round(Math.abs(b)):40:Math.round(Math.abs(b));if(this.density>360)this.density=360;this.activeId=0;this.redraw()};a.getDensity=function(){return this.density};a.range=1.3;a.setRange=function(b){this.range=Math.abs(b);this.redraw()};a.getRange=function(){return this.range};a.speed=2;a.setSpeed=function(b){this.speed=Math.round(Math.abs(b))};a.getSpeed=function(){return this.speed};
a.fps=24;a.setFPS=function(b){this.fps=Math.round(Math.abs(b));this.reset()};a.getFPS=function(){return this.fps};a.getRGB=function(b){b=b.charAt(0)==="#"?b.substring(1,7):b;return{r:parseInt(b.substring(0,2),16),g:parseInt(b.substring(2,4),16),b:parseInt(b.substring(4,6),16)}};a.draw=function(){var b=0,c,a,d,e,h,k,j,r=this.density,s=Math.round(r*this.range),l,i,q=0;i=this.cCon;var g=this.diameter;if(o===p[0]){i.clearRect(0,0,1E3,1E3);t(this.can,{width:g,height:g});for(t(this.cCan,{width:g,height:g});b<
r;){l=b<=s?1-1/s*b:l=0;k=270-360/r*b;j=k/180*Math.PI;i.fillStyle="rgba("+this.cRGB.r+","+this.cRGB.g+","+this.cRGB.b+","+l.toString()+")";switch(this.shape){case f[0]:case f[1]:c=g*0.07;e=g*0.47+Math.cos(j)*(g*0.47-c)-g*0.47;h=g*0.47+Math.sin(j)*(g*0.47-c)-g*0.47;i.beginPath();this.shape===f[1]?i.arc(g*0.5+e,g*0.5+h,c*l,0,Math.PI*2,false):i.arc(g*0.5+e,g*0.5+h,c,0,Math.PI*2,false);break;case f[2]:c=g*0.12;e=Math.cos(j)*(g*0.47-c)+g*0.5;h=Math.sin(j)*(g*0.47-c)+g*0.5;u(i,e,h,j);i.fillRect(e,h-c*0.5,
c,c);break;case f[3]:case f[4]:a=g*0.3,d=a*0.27,e=Math.cos(j)*(d+(g-d)*0.13)+g*0.5,h=Math.sin(j)*(d+(g-d)*0.13)+g*0.5,u(i,e,h,j),this.shape===f[3]?i.fillRect(e,h-d*0.5,a,d):(c=d*0.55,i.moveTo(e+c,h-d*0.5),i.lineTo(e+a-c,h-d*0.5),i.quadraticCurveTo(e+a,h-d*0.5,e+a,h-d*0.5+c),i.lineTo(e+a,h-d*0.5+d-c),i.quadraticCurveTo(e+a,h-d*0.5+d,e+a-c,h-d*0.5+d),i.lineTo(e+c,h-d*0.5+d),i.quadraticCurveTo(e,h-d*0.5+d,e,h-d*0.5+d-c),i.lineTo(e,h-d*0.5+c),i.quadraticCurveTo(e,h-d*0.5,e+c,h-d*0.5))}i.closePath();i.fill();
i.restore();++b}}else{m(this.cont,{width:g,height:g});m(this.vml,{width:g,height:g});switch(this.shape){case f[0]:case f[1]:j="oval";c=140;break;case f[2]:j="roundrect";c=120;break;case f[3]:case f[4]:j="roundrect",c=300}a=d=c;e=500-d;for(h=-d*0.5;b<r;){l=b<=s?1-1/s*b:l=0;k=270-360/r*b;switch(this.shape){case f[1]:a=d=c*l;e=500-c*0.5-c*l*0.5;h=(c-c*l)*0.5;break;case f[0]:case f[2]:v&&(h=0,this.shape===f[2]&&(e=500-d*0.5));break;case f[3]:case f[4]:a=c*0.95,d=a*0.28,v?(e=0,h=500-d*0.5):(e=500-a,h=
-d*0.5),q=this.shape===f[4]?0.6:0}i=t(m(n("group",this.vml),{width:1E3,height:1E3,rotation:k}),{coordsize:"1000,1000",coordorigin:"-500,-500"});i=m(n(j,i,{stroked:false,arcSize:q}),{width:a,height:d,top:h,left:e});n("fill",i,{color:this.color,opacity:l});++b}}this.tick(true)};a.clean=function(){if(o===p[0])this.con.clearRect(0,0,1E3,1E3);else{var b=this.vml;if(b.hasChildNodes())for(;b.childNodes.length>=1;)b.removeChild(b.firstChild)}};a.redraw=function(){this.clean();this.draw()};a.reset=function(){typeof this.timer===
"number"&&(this.hide(),this.show())};a.tick=function(b){var a=this.con,f=this.diameter;b||(this.activeId+=360/this.density*this.speed);o===p[0]?(a.clearRect(0,0,f,f),u(a,f*0.5,f*0.5,this.activeId/180*Math.PI),a.drawImage(this.cCan,0,0,f,f),a.restore()):(this.activeId>=360&&(this.activeId-=360),m(this.vml,{rotation:this.activeId}))};a.show=function(){if(typeof this.timer!=="number"){var a=this;this.timer=self.setInterval(function(){a.tick()},Math.round(1E3/this.fps));m(this.cont,{display:"block"})}};
a.hide=function(){typeof this.timer==="number"&&(clearInterval(this.timer),delete this.timer,m(this.cont,{display:"none"}))};a.kill=function(){var a=this.cont;typeof this.timer==="number"&&this.hide();o===p[0]?(a.removeChild(this.can),a.removeChild(this.cCan)):a.removeChild(this.vml);for(var c in this)delete this[c]};w.CanvasLoader=k})(window);

