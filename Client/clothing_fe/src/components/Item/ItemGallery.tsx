
//
import './Css/ItemGallery.css'
//
import React, { useState, useRef, useEffect } from "react";
import SliderPackage, { type Settings } from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

type ImageGalleryItem = {
    id: number
    displayOrder: number
    imageURL: string | null
}

type ItemGalleryProps = {
    images: ImageGalleryItem[]
}
const Slider =
    (SliderPackage as unknown as { default: typeof SliderPackage }).default ||
    SliderPackage;

export default function ItemGallery({ images }: ItemGalleryProps) {
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
        slidesToShow: Math.min(5, images.length),
        swipeToSlide: true,
        focusOnSelect: true,
    };

    if (!images || images.length === 0) {
        return <p>Không có ảnh sản phẩm</p>;
    }

    const sortedImages = [...images].sort((a, b) => a.displayOrder - b.displayOrder);

    return (
        <>
        <div className="">
            <div className="max-w-xl mx-auto space-y-4">
                <Slider ref={sliderRef1} {...mainSettings}>
                    {sortedImages.map((img)=>
                        <div key={img.id} className="bg-amber-50">
                            <img className='img-gallery' src={img.imageURL ?? ''} alt='product image'/>
                        </div>
                    )}
                </Slider>

                <Slider ref={sliderRef2} {...navSettings}>
                    {sortedImages.map((img)=>
                        <div key={img.id} className="">
                            <img src={img.imageURL ?? ''} alt='product image' className='thumb-gallery'/>
                        </div>
                    )}
                </Slider>
            </div>
        </div>
        </>
    );
}