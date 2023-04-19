import React from 'react';
import PropTypes from 'prop-types';
import './LocationCard.css';


const LocationCard = ({locationData: locationData, truckData, selected, handleClickLocation}) => {
    return (
        <div className="location-card" onClick={handleClickLocation}>
            <div className="card-body">
                <div className="card-subtitle">{locationData.identifier}</div>
            </div>
            <div className="image"
                 style={{backgroundImage: `url("${process.env.REACT_APP_API_HOST}Image?firetruck=${truckData.identifier}&location=${locationData.identifier}")`}}/>
        </div>
    );
};

LocationCard.propTypes = {
    locationData: PropTypes.shape({
        identifier: PropTypes.string.isRequired,
    }).isRequired,
    selected: PropTypes.bool.isRequired,
};

export default LocationCard;
