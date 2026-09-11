import React, { useRef } from "react";
import SliderPackage from "react-slick";
const Slider = (SliderPackage as any).default || SliderPackage;
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import { Button } from "@mui/material";

// Interface định kiểu dữ liệu cho TypeScript
interface ReviewData {
  name: string;
  img: string;
  review: string;
}


const data: ReviewData[] = [
  {
    name: "John Morgan",
    img: "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?auto=format&fit=crop&q=80&w=200",
    review: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
  },
  {
    name: "Ellie Anderson",
    img: "https://images.unsplash.com/photo-1494790108377-be9c29b29330?auto=format&fit=crop&q=80&w=200",
    review: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
  },
  {
    name: "Nia Adebayo",
    img: "https://images.unsplash.com/photo-1570295999919-56ceb5ecca61?auto=format&fit=crop&q=80&w=200",
    review: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
  },
  {
    name: "Rigo Louie",
    img: "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?auto=format&fit=crop&q=80&w=200",
    review: "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
  },
];


function Testing() {
   const sliderRef = useRef<Slider | null>(null);

    const next = () => {
        sliderRef.current?.slickNext();
    };

    const previous = () => {
        sliderRef.current?.slickPrev();
    };
    const settings = {
        dots: true,
        infinite: true,
        speed: 1000,
        slidesToShow: 3,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000,
        cssEase: "linear",
        pauseOnHover: true,
        responsive: [
        {
            breakpoint: 1024,
            settings: {
            slidesToShow: 2,
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

    return (
        <div className="w-4/5">
            <div className="justify-end flex gap-2 p-2">
                 <div style={{ textAlign: "center" }} 
                  className="mt-6">
                    <Button variant="outlined" onClick={previous}>
                        Previous
                    </Button>
                    <Button variant="outlined" onClick={next}>
                        Next
                    </Button>
                </div>
            </div>
            <div className="">
                <Slider ref={sliderRef} {...settings}>
                {data.map((d) => (
                <div key={d.name} className="px-2">
                    <div className="bg-white h-[50vh] text-black 
                    rounded-xl shadow-md overflow-hidden">
                        
                        <div className="h-56 bg-indigo-500 flex justify-center items-center rounded-t-xl">
                            <img src={d.img} alt={d.name} className="h-44 w-44 rounded-full object-cover border-4 border-white" />
                        </div>

                        <div className="flex flex-col items-center justify-center gap-4 p-4">
                            <p className="text-xl font-semibold">{d.name}</p>
                            <p className="text-center text-sm text-gray-600 line-clamp-3">{d.review}</p>
                            <button className="bg-indigo-500 hover:bg-indigo-600 text-white text-lg px-6 py-1.5 rounded-xl transition-colors">
                                Read More
                            </button>
                        </div>
                    </div>
                </div>
                ))}
                </Slider>
            </div>
        </div>
    );
}

export default Testing;