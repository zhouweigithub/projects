    var YunmallIFrame = function(){
    
	var  me = this;
	var  https = 0;
	this.FrameName = 'YunmallIFrame';
	this.ExtraFlag={t:new Date().getTime()};
	this.Type = 2;
	this.Url = '';
	
	this.FormData={};
	
	this.init = function(obj) {
			
			createIFrame(me.FrameName);
			
			var _myIframeForm = createForm(me.loginFormId);
			
			var request   = makeRequest(obj);
			
			request["encoding"] = "UTF-8";
			
			if (me.feedBackUrl) {
				request["url"] = makeURL(me.feedBackUrl, {
					"framelogin": 1
					//"callback": "parent." + me.name + ".feedBackUrlCallBack"
				});
				request["returntype"] = "META"
			} else {
				//request["callback"] = "parent." + me.name + ".loginCallBack";
				request["returntype"] = "IFRAME";
				request["setdomain"] = me.setDomain ? 1 : 0
			}
			for (var key in me.loginExtraQuery) {
				if (typeof me.loginExtraQuery[key] == "function") continue;
				request[key] = me.loginExtraQuery[key]
			}
			for (var name in request) {
				_myIframeForm.addInput(name, request[name])
			}
			
			
			for(var fj in  me.FormData){
				_myIframeForm.addInput(me.FormData[fj].name, me.FormData[fj].value,me.FormData[fj].type);
			}
			me.FormData = {};
			
			
			
			var action = (me.Type & https) ? me.Url.replace(/^http:/, "https:") : me.Url;
			action = makeURL(action, objMerge({
				"client": '1.0'
			},
			me.ExtraFlag));
			
			_myIframeForm.method = 'post';
			
			_myIframeForm.action = action;
			
			_myIframeForm.target = me.FrameName;
			
			var result = true;
			
			try {
				
				_myIframeForm.submit();
			} catch(e) {
				
				removeNode(me.FrameName);
				result = false
			}
			setTimeout(function() {
				removeNode(_myIframeForm)
			},
			10);
		return result;
	};
	
	this.setUrl = function(url) {
        me.Url = url
    };
	this.setExtra = function(Extra){
          me.ExtraFlag = Extra
	} 
	
	this.setFormData = function(FormData){
		  me.FormData = FormData;
	}
	
	this.CallBack = function(){
	
	}
	
	this.error = function(){
	
	}
	
	var makeURL = function(url, request) {
        return url + urlAndChar(url) + httpBuildQuery(request)
    };
	var urlAndChar = function(url) {
        return (/\?/.test(url) ? "&": "?")
    };
	var httpBuildQuery = function(obj) {
        if (typeof obj != "object") return "";
        var arr = new Array();
        for (var key in obj) {
            if (typeof obj[key] == "function") continue;
            arr.push(key + "=" + urlencode(obj[key]))
        }
        return arr.join("&")
    };
    var urlencode = function(str) {
        return encodeURIComponent(str)
    };
	var objMerge = function(obj1, obj2) {
        for (var item in obj2) {
            obj1[item] = obj2[item]
        }
        return obj1
    };
	
	
	 var createForm = function(formName, display) {
			if (display == null) display = 'none';
			removeNode(formName);
			var form = document.createElement('form');
			form.height = 0;
			form.width = 0;
			form.style.display = display;
			form.name = formName;
			form.id = formName;
			appendChild(document.body, form);
			document.forms[formName].name = formName;
			var inputarray=['text','radio','checkbox','hidden','select'],inputlist ={};
			for(var _t in inputarray){
				inputlist[inputarray[_t]]=_t;
			}
			form.addInput = function(name, value, type) {
				TagName = "input";

				if (type == null) type = 'text'  ;
				if(type=='textarea'){ TagName = "textarea"; }
				/*var _name = this.getElementsByTagName('input')[name];
				if (_name) {
					//console.log(_name);
					this.removeChild(_name)
				}*/
				var _name = document.createElement(TagName);
				this.appendChild(_name);
				_name.id = name;
				_name.name = name;
				if(typeof(inputlist[type])!=='undefined'){
					_name.type = "text";
				}
				
				_name.value = value
			};
		return form;
    };
	
	var makeRequest = function(obj) {
		    if("object" != typeof obj) return {};
         return obj;
    };
   
	var createIFrame = function(frameName, src) {
			if (src == null) src = "javascript:false;";
			removeNode(frameName);
			var frame = document.createElement('iframe');
			frame.height = 0;
			frame.width = 0;
			frame.style.display = 'none';
			frame.name = frameName;
			frame.id = frameName;
			frame.src = src;
			appendChild(document.body, frame);
			window.frames[frameName].name = frameName;
			
		return frame;
	};
	
	var removeNode = function(el) {
			try {
				if (typeof(el) === 'string') el = me.$(el);
				el.parentNode.removeChild(el)
			} catch(e) {}
	};
	
	this.$ = function(id) {
        return document.getElementById(id)
    }
	
	var appendChild = function(parentObj, element) {
	     parentObj.appendChild(element)
	};

   
   
   
}