import place from '../../assets/place.webp'
import item from '../../assets/placeCat.webp'
//
import './Css/ItemGallery.css'
//
import React, { useState, useRef, useEffect } from "react";
import SliderPackage, { type Settings } from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

// Fix ESM export resolution
const Slider =
  (SliderPackage as unknown as { default: typeof SliderPackage }).default ||
  SliderPackage;

export default function ItemGallery() {
  const [nav1, setNav1] = useState<SliderPackage | undefined>(undefined);
  const [nav2, setNav2] = useState<SliderPackage | undefined>(undefined);

  const sliderRef1 = useRef<SliderPackage | null>(null);
  const sliderRef2 = useRef<SliderPackage | null>(null);

  useEffect(() => {
    if (sliderRef1.current) setNav1(sliderRef1.current);
    if (sliderRef2.current) setNav2(sliderRef2.current);
  }, []);

  

  const mainSettings: Settings = {
    asNavFor: nav2 || undefined,
    dots: false,
    arrows: false,
    autoplay: true,
    autoplaySpeed: 3000,
    cssEase: "linear",
    pauseOnHover: true,
    infinite: true,
    slidesToShow: 1,
    responsive: [
            {
                breakpoint: 1024,
                settings: {
                slidesToShow: 1,
                dots: true
                },
            },
            {
                breakpoint: 640,
                settings: {
                slidesToShow: 1,
                },
            },
        ],
    };

    const navSettings: Settings = {
        asNavFor: nav1 || undefined, 
        slidesToShow: 5,
        swipeToSlide: true,
        focusOnSelect: true,
    };

    
    return (
        <>
        <div className="">
            <div className="max-w-xl mx-auto space-y-4">
                <Slider ref={sliderRef1} {...mainSettings}>
                    <div className="bg-amber-50">
                        <img className='img-gallery' src={place} alt='product image'/>
                    </div>
                    <div className="bg-amber-50">
                        <img className='img-gallery' src={item} alt='product image'/>
                    </div>
                    <div className="bg-amber-50">
                        <img className='img-gallery' src={place} alt='product image'/>
                    </div>
                </Slider>

                <Slider ref={sliderRef2} {...navSettings}>
                    <div className="">
                        <img src={item} alt='product image' className='thumb-gallery'/>
                    </div>
                    <div className="">
                        <img src={item} alt='product image' className='thumb-gallery'/>
                    </div>
                    <div className="">
                        <img src={item} alt='product image' className='thumb-gallery'/>
                    </div>
                </Slider>
            </div>
        </div>
        </>
    );
}