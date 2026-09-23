import './css//Banner.css'
import React, { useEffect, useState } from "react"
import CircleIcon from '@mui/icons-material/Circle';
import ArrowForwardIosIcon from '@mui/icons-material/ArrowForwardIos';
import ArrowBackIosNewIcon from '@mui/icons-material/ArrowBackIosNew';
import CircleOutlinedIcon from '@mui/icons-material/CircleOutlined';
import api from '../../api/ApiHandler';

type BannerItem  = {
    imageURL: string
    alt: string
}

export default function Banner(){

    const [banners, setBanner] = useState<BannerItem[]>([]);
    const [imgIndex, setImgIndex] = useState(0);

    useEffect(()=>{
        async function fetchBanners() {
            try{
                const {data} = await api.get(`/BannerModels/listing`);
                setBanner(data.results ?? data);
            } catch(error){
                console.error(error);
            }
        }

        fetchBanners()
    }, []);

    
    function showNextImage() {
        setImgIndex(index => 
            (index === banners.length-1 ? 0 : index + 1 ))
    }

    function showPrevImage() {
        setImgIndex(index => 
        (index === 0 ? banners.length-1 : index -1 ))
    }

    return(
        <>
        <section
        aria-label="Image Slider"
        style={{ width: "100%", height: "100%", position: "relative" }}
        >
            <a href="#after-image-slider-controls" className="skip-link">
                Skip Image Slider Controls
            </a>
            <div
                style={{
                width: "100%",
                height: "100%",
                display: "flex",
                overflow: "hidden",
                }}
            >
                {banners.map((banner, index) =>
                    <img
                    key={index}
                    src={banner.imageURL}
                    aria-hidden={imgIndex !== index}
                    className="img-slider-img"
                    style={{ translate: `${-100 * imgIndex}%` }}
                />
                
                )}


               
            </div>
                <button
                    onClick={showPrevImage}
                    className="img-slider-btn"
                    style={{ left: 0 }}
                    aria-label="View Previous Image"
                >
                    <ArrowBackIosNewIcon aria-hidden color="primary"/>
                </button>
                <button
                    onClick={showNextImage}
                    className="img-slider-btn"
                    style={{ right: 0 }}
                    aria-label="View Next Image"
                >
                    <ArrowForwardIosIcon aria-hidden color="primary"/>
                </button>
                <div
                    style={{
                    position: "absolute",
                    bottom: ".5rem",
                    left: "50%",
                    translate: "-50%",
                    display: "flex",
                    gap: ".25rem",
                    }}
                >
                    {banners.map((_, index) => (
                    <button
                        key={index}
                        className="img-slider-dot-btn"
                        aria-label={`View Image ${index + 1}`}
                        onClick={() => setImgIndex(index)}
                    >
                        {index === imgIndex ? (
                        <CircleIcon aria-hidden color="primary"/>
                        ) : (
                        <CircleOutlinedIcon aria-hidden color="primary"/>
                        )}
                    </button>
                    ))}
                </div>
            <div id="after-image-slider-controls" />
        </section>
        </>
    )
}