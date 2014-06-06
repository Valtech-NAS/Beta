/*
 *  jQuery Password Strength - v0.0.1
 *
 *  Made by Henry Charge
 *  Under MIT License
 */
// the semi-colon before function invocation is a safety net against concatenated
// scripts and/or other plugins which may not be closed properly.
;(function ( $, window, document, undefined ) {

		// undefined is used here as the undefined global variable in ECMAScript 3 is
		// mutable (ie. it can be changed by someone else). undefined isn't really being
		// passed in so we can ensure the value of it is truly undefined. In ES5, undefined
		// can no longer be modified.

		// window and document are passed through as local variable rather than global
		// as this (slightly) quickens the resolution process and can be more efficiently
		// minified (especially when both are regularly referenced in your plugin).

		// Create the defaults once
		var pluginName = "pwStrengthManager",
				defaults = {
  				password: "",
          blackList : [],
          minChars : "",
          maxChars : "",
          advancedStrength : false
		    };

		// The actual plugin constructor
		function Plugin ( element, options ) {
				this.element = element;
				// jQuery has an extend method which merges the contents of two or
				// more objects, storing the result in the first object. The first object
				// is generally empty as we don't want to alter the default options for
				// future instances of the plugin
				this.settings = $.extend( {}, defaults, options );
				this._defaults = defaults;
				this._name = pluginName;
				this.init();
        this.info = "";
        this.className = "";
		}

		Plugin.prototype = {
				init: function() {
          var errors = this.customValidators();

          if ("" == this.settings.password) {
            this.info = "Password cannot be empty";
            this.className = "strength-weak";
          } else if (errors == 0) {
            var strength = zxcvbn(this.settings.password, this.settings.blackList);

            switch (strength.score) {
              case 0:
                this.info = "Very weak";
                this.className = "strength-weak";
                break;
              case 1:
                this.info = "Weak";
                this.className = "strength-weak";
                break;
              case 2:
                this.info = "Medium";
                this.className = "strength-medium";
                break;
              case 3:
                this.info = "Strong";
                this.className = "strength-strong";
                break;
              case 4:
                this.info = "Strongest";
                this.className = "strength-strong";
                break;
            }
          }

          $(this.element).html(this.info).removeClass().addClass(this.className);
        },
				minChars: function() {
          if (this.settings.password.length < this.settings.minChars) {
            this.info = "Must be at least " + this.settings.minChars + " characters";
            return false;
          } else {
            return true;
          }
        },
        customValidators: function() {
          var err = 0;

          if (this.settings.minChars != "") {
            if (!this.minChars()) {
              err++;
            }
          }

          return err;
        }
		};

		// A really lightweight plugin wrapper around the constructor,
		// preventing against multiple instantiations
		$.fn[pluginName] = function (options) {
      this.each(function() {
        $.data(this, "plugin_" + pluginName, new Plugin(this, options));
      });
      return this;
    };

})( jQuery, window, document );
