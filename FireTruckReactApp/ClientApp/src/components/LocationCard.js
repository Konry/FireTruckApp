import React from 'react';
import PropTypes from 'prop-types';
import './LocationCard.css';


const LocationCard = ({ locationData: locationData, selected, handleClickLocation }) => {
    return (
        <div className="location-card" onClick={handleClickLocation}>
            <div className="card-body">
                <div className="card-subtitle">{locationData.identifier}</div>
            </div>
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
