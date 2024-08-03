(function ($) {
	"use strict";


	/*----------------------------
    Responsive menu Active
    ------------------------------ */
	$(".mainmenu ul#primary-menu").slicknav({
		allowParentLinks: true,
		prependTo: '.responsive-menu',
	});
	
	/*----------------------------
    START - Scroll to Top
    ------------------------------ */
	$(window).on('scroll', function () {
		console.log(scrollY);
		if ($(this).scrollTop() > 600) {
			$('#To-Top').fadeIn();
		} else {
			$('#To-Top').fadeOut();
		}
	});
	$('.ToTop-button').on('click', function () {
		$('html, body').animate({scrollTop : 0},2000);
		return false;
	});
	$('.menu-area ul > li > .theme-btn').on('click', function () {
		$('.buy-ticket').show();
		return false;
	});
	$('.buy-ticket .buy-ticket-area > a').on('click', function () {
		$('.buy-ticket').hide();
		return false;
	});
	$('.login-popup').on('click', function () {
		$('.login-area').show();
		return false;
	});
	$('.login-box > a').on('click', function () {
		$('.login-area').hide();
		return false;
	});
	
	/*----------------------------
    START - Slider activation
    ------------------------------ */
	var heroSlider = $('.hero-area-slider');
	heroSlider.owlCarousel({
		loop:true,
		dots: true,
		autoplay: false,
		autoplayTimeout:4000,
		nav: false,
		items: 1,
		responsive:{
			992:{
				dots: false,
			}
		}
	});
	heroSlider.on('changed.owl.carousel', function(property) {
		var current = property.item.index;
		var prevRating = $(property.target).find(".owl-item").eq(current).prev().find('.hero-area-slide').html();
		var nextRating = $(property.target).find(".owl-item").eq(current).next().find('.hero-area-slide').html();
		$('.thumb-prev .hero-area-slide').html(prevRating);
		$('.thumb-next .hero-area-slide').html(nextRating);
	});
	$('.thumb-next').on('click', function() {
		heroSlider.trigger('next.owl.carousel', [300]);
		return false;
	});
	$('.thumb-prev').on('click', function() {
		heroSlider.trigger('prev.owl.carousel', [300]);
		return false;
	});
	var newsSlider = $('.news-slider');
	newsSlider.owlCarousel({
		loop:true,
		dots: true,
		autoplay: false,
		autoplayTimeout:4000,
		nav: false,
		items: 1,
		responsive:{
			992:{
				dots: false,
			}
		}
	});
	newsSlider.on('changed.owl.carousel', function(property) {
		var current = property.item.index;
		var prevRating = $(property.target).find(".owl-item").eq(current).prev().find('.single-news').html();
		var nextRating = $(property.target).find(".owl-item").eq(current).next().find('.single-news').html();
		$('.news-prev .single-news').html(prevRating);
		$('.news-next .single-news').html(nextRating);
	});
	$('.news-next').on('click', function() {
		newsSlider.trigger('next.owl.carousel', [300]);
		return false;
	});
	$('.news-prev').on('click', function() {
		newsSlider.trigger('prev.owl.carousel', [300]);
		return false;
	});
	var videoSlider = $('.video-slider');
	videoSlider.owlCarousel({
		loop:true,
		dots: true,
		autoplay: false,
		autoplayTimeout:4000,
		nav: false,
		responsive:{
			0:{
				items: 1,
				margin: 0
			},
			576:{
				items: 2,
				margin: 30
			},
			768:{
				items: 3,
				margin: 30
			},
			992:{
				items: 4,
				margin: 30
			}
		}
	});
	

	/*----------------------------
    START - Isotope
    ------------------------------ */
    jQuery(".portfolio-item").isotope();
    $(".portfolio-menu li").on("click", function(){
      $(".portfolio-menu li").removeClass("active");
      $(this).addClass("active");
      var selector = $(this).attr('data-filter');
      $(".portfolio-item").isotope({
        filter: selector
      })
    });
	
	/*----------------------------
    START - Preloader
    ------------------------------ */
	jQuery(window).load(function(){
		jQuery("#preloader").fadeOut(500);
	});


	$('#purchase-modal').on('shown.bs.modal', function (e) {
		$('html, body').animate({ scrollTop: 0 }, 300);
		return false;
	})

	$('.open-feedback').on('click', function (e) {
		$('.rating-container').fadeIn(500);
	})
	$('.close-Icon').on('click', function (e) {
		$('.rating-container').fadeOut(500);
	})
	$('#AddTestimonial').click(AddTestimonial);
	function AddTestimonial() {
		// Clear previous error messages
		$('#comment-error').text('');
		$('#rating-error').text('');

		var comment = $('#Comment').val();
		var rating = $('input[name="star"]:checked').val();

		// Validate the form fields
		var isValid = true;

		if (!rating) {
			$('#rating-error').text('Please select a rating!');
			isValid = false;
		}

		if (!comment) {
			$('#comment-error').text('Please enter a comment!');
			isValid = false;
		}

		if (!isValid) {
			return;
		}
		// Send the form data via AJAX request
		$.ajax({
			url: '/Testimonial/Create',
			method: 'POST',
			data: {
				Comment: comment,
				Rating: rating
			},
			success: function (response) {
				if (response.success) {
					Swal.fire({
						icon: 'success',
						text: response.message
					});
				} else {
					Swal.fire({
						icon: 'error',
						text: response.message
					});
				}
				$('.rating-container').fadeOut(500);
			},
			error: function (xhr, status, error) {
				Swal.fire({
					icon: 'error',
					text: 'Something went wrong. Please try again later.'
				});
			}
		});
	}

}(jQuery));