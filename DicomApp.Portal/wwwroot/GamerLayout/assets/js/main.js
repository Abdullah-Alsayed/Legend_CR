
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
	$('#openBuy-ticket').on('click', function () {
		let isAuthenticated =$(this).data('isauthenticated').toString();
		let authenticated = authenticatedCheck(isAuthenticated);
		if (authenticated)
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

	AOS.init();

	/*----------------------------
    START - Slider activation
    ------------------------------ */
	var heroSlider = $('.hero-area-slider');
	heroSlider.owlCarousel({
		loop:true,
		dots: true,
		autoplay: false,
		autoplayTimeout:8000,
		nav: false,
		items: 1,
		responsive:{
			992:{
				dots: false,
			}
		}
	});
	document.addEventListener('scroll', function () {
		const heroArea = document.querySelector('.hero-area' );
		if (heroArea != null) {

		// Get the scroll position
		const scrollY = window.scrollY;

		// Adjust these multipliers to change the speed and direction of the background movement
		const xMultiplier = 0.1;
		const yMultiplier = 0.2;

		// Calculate new background positions
		const backgroundPosX = scrollY * xMultiplier;
		const backgroundPosY = scrollY * yMultiplier;

		// Set the new background positions
		heroArea.style.backgroundPosition = `${backgroundPosX}px ${backgroundPosY}px`;
        }
	});

	document.addEventListener('scroll', function () {
		const portfolioArea = document.querySelector('.portfolio-area');

		// Get the scroll position
		const scrollY = window.scrollY;

		// Start changing the background position after scrolling 650 pixels
		if (scrollY > 650 && portfolioArea != null) {
			// Adjust these multipliers to change the speed and direction of the background movement
			const xMultiplier = 0.1;
			const yMultiplier = 0.2;

			// Calculate the new background positions
			const backgroundPosX = 100 - ((scrollY - 650) * xMultiplier); // 100% to 0%
			const backgroundPosY = ((scrollY - 650) * yMultiplier); // 0% to 100%

			// Ensure the position doesn't go below 0% or above 100%
			const finalBackgroundPosX = Math.max(0, backgroundPosX);
			const finalBackgroundPosY = Math.min(100, backgroundPosY);

			// Set the new background positions
			portfolioArea.style.backgroundPosition = `${finalBackgroundPosX}% ${finalBackgroundPosY}%`;
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
		autoplayTimeout:6000,
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
	$('.closePush-Icon').on('click', function (e) {
		$('.form-container').fadeOut(300);
	})

	$('#Push-Btn').on('click', function (e) {
		$('.form-container').fadeIn(400);
	})
	$('#AddTestimonial').click(AddTestimonial);
	$('#Login-Form').on('submit', function (e) {
		// Show the loader
		$('#login-text').hide();
		$('#login-loader').show();
		// Disable the submit button to prevent multiple clicks
		$('.login-button').prop('disabled', true);
	});

	//$('#Account-checkout').on('click', function(), AccountCheckout());

	$('#UpdateUserData').on('show.bs.modal', function (e) {
		$('#UpdateUserLoader').show();
		$('#Gamer-Form').addClass('d-none');

		var userId = $('#UserID').val();
		$.ajax({
			url: '/User/GetUserData',
			type: 'GET',
			data: { Id: userId },
			dataType: 'json',
			success: function (response) {
				if (response) {

 					$('#Name').val(response.name);
					$('#Email').val(response.email);
					$('#PhoneNumber').val(response.phoneNumber);
					$('#TelegramUserName').val(response.telegramUserName);
					$('#Age').val(response.age);
					$('#Gender').val(response.gender);

					$('#UpdateUserLoader').hide();
					$('#Gamer-Form').removeClass('d-none');
				}
			},
			error: function () {
				console.error('An error occurred while fetching user data.');

				$('#UpdateUserLoader').hide();
				$('#Gamer-Form').removeClass('d-none');
			}
		});
	});
	var translations = {
		en: {
			selectRating: "Please select a rating!",
			enterComment: "Please enter a comment!",
			success: "Your testimonial has been added successfully!",
			error: "Something went wrong. Please try again later."
		},
		ar: {
			selectRating: "يرجى اختيار تقييم!",
			enterComment: "يرجى إدخال تعليق!",
			success: "تم إضافة رائيك بنجاح!",
			error: "حدث خطأ ما. حاول مرة أخرى لاحقًا."
		}
	};

	userLanguage = (userLanguage && userLanguage.length >= 2) ? userLanguage.substring(0, 2) : "en";

	function AddTestimonial() {
		// Clear previous error messages
		$('#comment-error').text('');
		$('#rating-error').text('');

		var comment = $('#Comment').val();
		var rating = $('input[name="star"]:checked').val();

		// Validate the form fields
		var isValid = true;

		if (!rating) {
			$('#rating-error').text(translations[userLanguage].selectRating);
			isValid = false;
		}

		if (!comment) {
			$('#comment-error').text(translations[userLanguage].enterComment);
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
						text: translations[userLanguage].success
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
					text: translations[userLanguage].error
				});
			}
		});
	}

}(jQuery));