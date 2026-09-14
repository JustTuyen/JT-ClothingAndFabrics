
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Typography from '@mui/material/Typography';
import CardActionArea from '@mui/material/CardActionArea';

//
import Cat from '../../assets/placeCat.webp'
//
export default function CategoryLog(){
    return(
        <>
        <div className="flex flex-col gap-8 items-center">
            <div className="flex justify-center">
                <p className="text-[18px] font-bold md:text-[24px]">DANH MỤC SẢN PHẨM</p>
            </div>
            <div className="p-4 grid 
            grid-cols-2
            md:grid-cols-4
            gap-4">
                <Card sx={{ maxWidth: 300, border: 'none', boxShadow: 'none'}}>
                    <CardActionArea>
                        <CardMedia
                        component="img"
                        height="140"
                        image={Cat}
                        alt="green iguana"
                        />
                        <CardContent sx={{textAlign: 'center'}}>
                            <Typography gutterBottom variant="h5" component="div">
                                Áo Nam
                            </Typography>
                        </CardContent>
                    </CardActionArea>
                </Card>
                
            </div>
        </div>
        </>
    )
}